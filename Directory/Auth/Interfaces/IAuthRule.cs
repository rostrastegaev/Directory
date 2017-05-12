using Common;

namespace Auth
{
    public interface IAuthRule
    {
        Result Validate(IAuthRequest request);
    }
}
