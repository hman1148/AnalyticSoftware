using Framework;
using Microsoft.AspNetCore.SignalR;

namespace WebsiteTest.Controllers
{
    public class TestProducts : IEndpointDefinition
    {

        private readonly ILogger<TestProducts> _logger;

        public TestProducts(ILogger<TestProducts> logger) 
        {
            _logger = logger;
        }  
        public void DefineEndpoint(IEndpointRouteBuilder app)
        {
            var endpoint = new Endpointbuilder()
                .WithRoute("/products")
                .WithHttpMethod(HttpMethod.Get)
                .WithAction(GetProducts)
                .Build();

            app.MapEndpoint(endpoint);
        }

        private async Task GetProducts(HttpContext context)
        {
            var responseMessage = "List of products";
            await context.Response.WriteAsJsonAsync(responseMessage);
        }
    }
}
