using Microsoft.IdentityModel.Tokens;
using System;

namespace Auth
{
    public class Token
    {
        private SecurityToken _token;
        private SecurityTokenHandler _handler;

        public DateTime ValidTo => _token.ValidTo;

        public Token(SecurityToken token, SecurityTokenHandler handler)
        {
            _token = token;
            _handler = handler;
        }

        public override string ToString() =>
            _handler.WriteToken(_token);
    }
}
