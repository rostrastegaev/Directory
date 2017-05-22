using Common;
using DAL;
using System.Threading.Tasks;

namespace Auth
{
    public interface IGrantProvider
    {
        string By { get; }
        Task<Result<AuthResponse>> Grant(SignInRequest request);
    }
}
