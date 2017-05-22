using DAL;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth
{
    class TokenCreator
    {
        private User _user;
        private AuthConfig _config;
        private JwtSecurityTokenHandler _handler;

        public TokenCreator(User user, AuthConfig config)
        {
            _user = user;
            _config = config;
            _handler = new JwtSecurityTokenHandler();
        }

        public Token CreateToken(bool isExpirationNeeded = true)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, _user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email)
            };
            var identity = new ClaimsIdentity(claims);

            var now = DateTime.UtcNow;
            JwtSecurityToken token = _handler.CreateJwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                subject: identity,
                notBefore: now,
                expires: isExpirationNeeded ? (DateTime?)now.AddHours(_config.Lifetime) : null,
                issuedAt: now,
                signingCredentials: _config.Credentials);
            return new Token(token, _handler);
        }
    }
}
