namespace IdentityAPI.Contracts.v1.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; init; }

        public AuthSuccessResponse(string token)
        {
            Token = token;
        }
    }
}
