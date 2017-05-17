using System.Threading.Tasks;
using Common;
using System;
using DAL;

namespace Auth
{
    public class RefreshTokenGrantProvider : IGrantProvider
    {
        private AuthConfig _config;

        public string By => "refresh_token";

        public RefreshTokenGrantProvider(AuthConfig config)
        {
            _config = config;
        }

        public async Task<Result<AuthResponse>> Grant(SignInRequest request)
        {
            return await Task.Run(() =>
            {
                User user;
                try
                {
                    var tokenReader = new TokenReader(request.Token);
                    user = tokenReader.ToUser();
                }
                catch (Exception)
                {
                    return Result<AuthResponse>.Error(ErrorCodes.AUTH_INVALID_TOKEN);
                }
                if (user == null)
                {
                    return Result<AuthResponse>.Error(ErrorCodes.NOT_AUTH);
                }

                var tokenCreator = new TokenCreator(user, _config);
                var accessToken = tokenCreator.CreateToken();
                return Result<AuthResponse>.Success(
                    new AuthResponse(accessToken.ToString(), null, accessToken.ValidTo));
            });
        }
    }
}
