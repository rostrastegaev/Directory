using System.Security.Claims;

namespace Auth
{
    public interface IAuthRequest
    {
        string Email { get; }
        string Password { get; }
        string Confirmation { get; }
    }
}
