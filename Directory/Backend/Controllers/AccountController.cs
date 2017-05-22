using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [AllowAnonymous]
    [Route("api/account")]
    public class AccountController
    {
        private IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequest request)
        {
            var result = await _authService.Register(request);
            return new JsonResult(result);
            
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody]SignInRequest request)
        {
            var result = await _authService.SignIn(request);
            return new JsonResult(result);
        }
    }
}
