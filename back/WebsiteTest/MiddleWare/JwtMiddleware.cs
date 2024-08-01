using AnalyticSoftware.Services;

namespace AnalyticSoftware.MiddleWare
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityService _securityService;

        public JwtMiddleware(RequestDelegate next, SecurityService securityService)
        {
            _next = next;
            _securityService = securityService;
        }

        public async Task Invoke(HttpContext ctx)
        {
            var token = ctx.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var principle = _securityService.ValidateToken(token);
                if (principle != null)
                {
                    ctx.User = principle;
                }
            }

            await _next(ctx);
        }
    }
}
