using Microsoft.AspNetCore.Http;
using System;

namespace Backend.Controllers
{
    public static class ControllerExtensions
    {
        public static string GetToken(this IHttpContextAccessor accessor)
        {
            HttpRequest request = accessor.HttpContext.Request;
            if (!request.Headers.ContainsKey("Authorization"))
            {
                throw new Exception("Unauthorized");
            }
            return request.Headers["Authorization"][0];
        }
    }
}
