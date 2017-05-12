using System.Security.Claims;
using Common;
using System.Threading.Tasks;

namespace Auth
{
    public interface IAuthService
    {
        Result<int> GetId(ClaimsPrincipal principal);
        Task<Result<IAuthResponse>> SignIn(IAuthRequest request, ClaimsPrincipal principal);
        Task<Result<IAuthResponse>> Register(IAuthRequest request, ClaimsPrincipal principal);
    }
}
