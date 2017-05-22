using Common;
using DAL;
using System.Threading.Tasks;

namespace Auth
{
    public interface IAuthService
    {
        Result<User> GetUser(string token);
        Task<Result<AuthResponse>> SignIn(SignInRequest request);
        Task<Result> Register(RegistrationRequest request);
    }
}
