using System;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Security.Cryptography;
using System.Linq;
using Common;

namespace Auth
{
    public class PasswordGrantProvider : IGrantProvider
    {
        private Encoding _encoding;
        private HashAlgorithm _hash;
        private IRepository<User> _usersRepo;
        private AuthConfig _config;

        public string By => "password";

        public PasswordGrantProvider(Encoding encoding,
            HashAlgorithm hash,
            IDataService dataService,
            AuthConfig config)
        {
            _config = config;
            _encoding = encoding;
            _hash = hash;
            _usersRepo = dataService.GetRepository<User>();
        }

        public async Task<Result<AuthResponse>> Grant(SignInRequest request)
        {
            User user = await _usersRepo.Get(
                u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Result<AuthResponse>.Error(ErrorCodes.AUTH_EMAIL_NOT_FOUND);
            }

            byte[] requestPassword = _hash.ComputeHash(_encoding.GetBytes(request.Password));
            if (!user.Password.SequenceEqual(requestPassword))
            {
                return Result<AuthResponse>.Error(ErrorCodes.AUTH_EMAIL_OR_PASSWORD_INCORRECT);
            }

            TokenCreator tokenCreator = new TokenCreator(user, _config);
            var accessToken = tokenCreator.CreateToken();
            var refreshToken = tokenCreator.CreateToken(false);
            return Result<AuthResponse>.Success(
                new AuthResponse(accessToken.ToString(), refreshToken.ToString(), accessToken.ValidTo));
        }
    }
}
