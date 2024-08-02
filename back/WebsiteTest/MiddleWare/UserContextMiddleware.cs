using AnalyticSoftware.Models;
using System.Security.Claims;

namespace AnalyticSoftware.MiddleWare
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;
        
        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            if (ctx.User.Identity.IsAuthenticated)
            {
                ClaimsIdentity? claimsIdentity = ctx.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                ctx.Items["UserId"] = userId;
                ctx.Items["UserEmail"] = userEmail;
            }
            await _next(ctx);
        }
    }
}
