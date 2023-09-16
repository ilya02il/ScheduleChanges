using JwtValidation.Messages;
using JwtValidation.Service;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAPI.GrpcClients;

public class JwtValidationServiceGrpcClient
{
    private readonly GrpcJwtValidationService.GrpcJwtValidationServiceClient _client;

    public JwtValidationServiceGrpcClient(GrpcJwtValidationService.GrpcJwtValidationServiceClient client)
    {
        _client = client;
    }

    public async Task<bool> ValidateJwtTokenAsync(string token,
        CancellationToken cancellationToken)
    {
        var request = new ValidateJwtTokenRequest()
        {
            Token = token
        };

        var response = await _client.ValidateJwtTokenAsync(request,
            cancellationToken: cancellationToken);

        return response.IsValid;
    }
}