using IdentityAPI.Configurations;
using IdentityAPI.Contracts;
using IdentityAPI.Data;
using IdentityAPI.Extensions;
using IdentityAPI.Models;
using IdentityAPI.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtConfiguration _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IdentityService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            //RoleManager<ApplicationRole> roleManager,
            JwtConfiguration jwtConfig,
            TokenValidationParameters tokenValidationParameters)
        {
            _context = context;
            _userManager = userManager;
            //_roleManager = roleManager;
            _jwtConfig = jwtConfig;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthenticationResult> LoginAsync(string username,
            string password,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(username)
                .WithCancellation(cancellationToken);

            if (user is null)
                return new(new[] { "User does not exists." });

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, password)
                .WithCancellation(cancellationToken);

            if (!isValidPassword)
                return new(new[] { "User's login or password is incorrect." });

            return await GenerateAuthResultForUserAsync(user, cancellationToken);
        }

        public async Task<RegistrationResult> RegisterUserAsync(Guid educOrgId,
            string username,
            string password,
            string role,
            CancellationToken cancellationToken = default)
        {
            var existingUser = await _userManager.FindByNameAsync(username)
                .WithCancellation(cancellationToken);

            if (existingUser is not null)
                return new(new[] { "The username is already exist." });

            var newUser = new ApplicationUser(username, educOrgId);

            var createdUser = await _userManager.CreateAsync(newUser, password)
                .WithCancellation(cancellationToken);

            if (!createdUser.Succeeded)
                return new(createdUser.Errors.Select(e => e.Description));

            if (role is not null)
                await _userManager.AddToRoleAsync(newUser, role);

            return new(true);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token,
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken is null)
                return new(new[] { "Invalid token" });

            var expiryDateUnix = long.Parse(validatedToken.Claims
                .Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc < DateTime.UtcNow)
                return new(new[] { "This token hasn't expired yet." });

            var jti = validatedToken.Claims
                .Single(c => c.Type == JwtRegisteredClaimNames.Jti)
                .Value;

            var storedRefreshToken = _context.RefreshTokens
                .FirstOrDefault(rt => rt.Token == refreshToken);

            if (storedRefreshToken is null)
                return new(new[] { "This refresh token does not exist." });

            if (DateTime.UtcNow > storedRefreshToken.ExpireDateTime)
                return new(new[] { "This token has expired. " });

            if (storedRefreshToken.IsInvalid)
                return new(new[] { "This refresh token has been invalitaded." });

            if (storedRefreshToken.IsUsed)
                return new(new[] { "This refresh token has been used." });

            if (storedRefreshToken.JwtId != jti)
                return new(new[] { "This refresh token hasn't been match this JWT." });

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);

            await _context.SaveChangesAsync(cancellationToken);

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(c => c.Type == "id").Value)
                .WithCancellation(cancellationToken);

            return await GenerateAuthResultForUserAsync(user, cancellationToken);
        }

        private async Task<AuthenticationResult> GenerateAuthResultForUserAsync(ApplicationUser user,
            CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("id", user.Id.ToString()),
                new("educ_org_id", user.EducOrgId.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user)
                .WithCancellation(cancellationToken);

            foreach (var role in userRoles)
                claims.Add(new(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtConfig.TokenLifetime),
                SigningCredentials = new(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = user.Id,
                CreationDateTime = DateTime.UtcNow,
                ExpireDateTime = DateTime.UtcNow.AddMonths(6)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            var cookies = new(string, string, CookieOptions)[]
            {
                new("X-Refresh-Token",
                    refreshToken.Token,
                    new()
                    {
                        HttpOnly = true,
                        //Secure = false,
                        //SameSite = SameSiteMode.None
                    }
                )
            };

            return new(true, tokenHandler.WriteToken(token), cookies);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = _tokenValidationParameters.Clone();
            tokenValidationParameters.ValidateLifetime = false;

            var principial = tokenHandler.ValidateToken(token,
                tokenValidationParameters,
                out var validatedToken);

            if (!IsJwtValidSecurityAlgorithm(validatedToken))
                return null;

            return principial;
        }

        private bool IsJwtValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
