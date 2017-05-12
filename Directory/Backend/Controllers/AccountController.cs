using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [AllowAnonymous]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]AuthRequest request)
        {
            var result = await _authService.Register(request, User);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody]AuthRequest request)
        {
            var result = await _authService.SignIn(request, User);
            return new JsonResult(result);
        }
    }
}
