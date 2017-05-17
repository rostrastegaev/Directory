using System;

namespace Auth
{
    public class AuthResponse : IAuthResponse
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime Expiration { get; }

        public AuthResponse(string accessToken, string refreshToken, DateTime expiration)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Expiration = expiration;
        }
    }
}
