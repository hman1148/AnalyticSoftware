using AnalyticSoftware.Models;
using AnalyticSoftware.Services;
using Framework;

namespace AnalyticSoftware.Controllers
{
    public class UserAuthController : IEndpointDefinition
    {

        private readonly UserService _userService;
        private readonly SecurityService _securityService;
        private readonly ILogger<UserAuthController> _logger;

        public UserAuthController(UserService userService, SecurityService securityService, ILogger<UserAuthController> logger)
        {
            _userService = userService;
            _securityService = securityService;
            _logger = logger;
        }

        public void DefineEndpoint(IEndpointRouteBuilder app)
        {
            var registerUserEndpoint = new Endpointbuilder()
                 .WithRoute("/register")
                 .WithHttpMethod(HttpMethod.Post)
                 .WithAction(RegisterUser)
                 .Build();

            var regiseterLoginEndpoint = new Endpointbuilder()
                .WithRoute("/login")
                .WithHttpMethod(HttpMethod.Post)
                .WithAction(LoginUser)
                .Build();

            app.MapEndpoint(registerUserEndpoint);
            app.MapEndpoint(regiseterLoginEndpoint);
            _logger.LogDebug("Registration and Login Endpoints defined");

        }

        private async Task RegisterUser(HttpContext ctx)
        {
            var registerRequest = await ctx.Request.ReadFromJsonAsync<RegisterRequest>();

            if (registerRequest == null)
            {
                ctx.Response.StatusCode = 400;
                DataResponse<string> dataResponse = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = "Failed to Register User",
                    Success = false
                };
                await ctx.Response.WriteAsJsonAsync(dataResponse);
                return;
            }

            var email = registerRequest.Email;
            var password = registerRequest.Password;
            var role = registerRequest.Role;
            var s3Bucket = registerRequest.S3Bucket;

            try
            {
                ctx.Response.StatusCode = 200;
                DataResponse<string> dataResponse = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = "Successfully Registered User",
                    Success = true
                };
                await ctx.Response.WriteAsJsonAsync(dataResponse);
                return;
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = 500;
                DataResponse<string> dataResponse = new DataResponse<string>
                {
                    Data = ex.Message,
                    Message = ex.Message,
                    Success = false
                };
                await ctx.Response.WriteAsJsonAsync(dataResponse);
            }
        }

        private async Task LoginUser(HttpContext ctx)
        {
            var loginRequest = await ctx.Request.ReadFromJsonAsync<LoginRequest>();

            if (loginRequest == null)
            {
                DataResponse<string> invalidLoginResponse = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = "Failed to Login User",
                    Success = false
                };
                await ctx.Response.WriteAsJsonAsync(invalidLoginResponse);
                return;
            }

            var email = loginRequest.Email;
            var password = loginRequest.Password;

            var user = await _userService.GetUserByEmail(email);
            if (user == null || _securityService.VerifyPassword(password, user.PasswordHash))
            {
                ctx.Response.StatusCode = 400;
                DataResponse<string> inValidDataResponse = new DataResponse<string>
                {
                    Data = string.Empty,
                    Message = "Failed to Validate User Password",
                    Success = false
                };
                await ctx.Response.WriteAsJsonAsync(inValidDataResponse);
                return;
            }
            var token = _securityService.GenerateToken(user);

            DataResponse<string> dataResponse = new DataResponse<string>
            {
                Data = token,
                Message = "Generated User Token",
                Success = true
            };
            await ctx.Response.WriteAsJsonAsync(dataResponse);
        }
    }
}
