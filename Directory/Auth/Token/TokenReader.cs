using DAL;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Auth
{
    public class TokenReader
    {
        private JwtSecurityToken _token;

        public TokenReader(string token)
        {
            _token = new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        public User ToUser()
        {
            var claimId = _token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Sid));
            var claimEmail = _token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email));
            if (!int.TryParse(claimId.Value, out int id))
            {
                return null;
            }
            return new User()
            {
                Id = id,
                Email = claimEmail.Value
            };
        }
    }
}
