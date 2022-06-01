using Grpc.Core;
using JwtValidation.Messages;
using JwtValidation.Service;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace IdentityAPI.GrpcServices
{
    public class JwtValidationService : GrpcJwtValidationService.GrpcJwtValidationServiceBase
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtValidationService(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async override Task<ValidateJwtTokenResponse> ValidateJwtToken(ValidateJwtTokenRequest request, ServerCallContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();

                    tokenHandler.ValidateToken(request.Token,
                        _tokenValidationParameters,
                        out var validatedToken);

                    return new ValidateJwtTokenResponse()
                    {
                        IsValid = true
                    };
                }
                catch
                {
                    return new ValidateJwtTokenResponse()
                    {
                        IsValid = false
                    };
                }
            });
        }
    }
}
