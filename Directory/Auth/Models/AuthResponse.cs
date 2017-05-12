namespace Auth
{
    public class AuthResponse : IAuthResponse
    {
        public string AccessToken { get; }

        public AuthResponse(string token)
        {
            AccessToken = token;
        }
    }
}
