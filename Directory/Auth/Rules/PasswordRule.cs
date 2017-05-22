using Common;

namespace Auth
{
    public class PasswordRule : IAuthRule
    {
        private AuthConfig _config;

        public PasswordRule(AuthConfig config)
        {
            _config = config;
        }

        public Result Validate(RegistrationRequest request)
        {
            Result result = new Result(true);
            if (!request.Password.Equals(request.Confirmation))
            {
                result.AddError(ErrorCodes.AUTH_PASSWORD_CONFIRMATION_NOT_EQ);
            }
            if (request.Password.Length < _config.PasswordLength)
            {
                result.AddError(ErrorCodes.AUTH_PASSWORD_LENGTH);
            }
            return result;
        }
    }
}
