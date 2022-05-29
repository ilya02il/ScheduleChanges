using IdentityAPI.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityAPI.Contracts
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> LoginAsync(string username,
            string password,
            CancellationToken cancellationToken);
        Task<AuthenticationResult> RefreshTokenAsync(string token,
            string refreshToken,
            CancellationToken cancellationToken);
        Task<RegistrationResult> RegisterUserAsync(Guid educOrgId,
            string username,
            string password,
            CancellationToken cancellationToken);
    }
}
