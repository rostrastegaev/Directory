using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [AllowAnonymous]
    [Route("views")]
    public class ViewsController
    {
        private IViewService _viewService;

        public ViewsController(IViewService viewService)
        {
            _viewService = viewService;
        }

        [HttpGet]
        [Route("{*url}")]
        public IActionResult GetView(string url)
        {
            return new FileStreamResult(_viewService.GetView($"views/{url}"), "text/html; charset=utf-8");
        }
    }
}
