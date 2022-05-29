namespace IdentityAPI.Contracts.v1.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }

        public AuthSuccessResponse(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
