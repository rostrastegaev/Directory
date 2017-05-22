using Common;

namespace Auth
{
    public interface IAuthRule
    {
        Result Validate(RegistrationRequest request);
    }
}
