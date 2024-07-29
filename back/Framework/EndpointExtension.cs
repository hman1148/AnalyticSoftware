

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Framework
{
    public static class EndpointExtension
    {   
        public static void MapEndpoint(this IEndpointRouteBuilder endpoints, Endpoint endpoint)
        {
            var httpMethods = new string[] { endpoint.HttpMethod.Method };

            endpoints.MapMethods(endpoint.Route, httpMethods, async context =>
            {
                await endpoint.Action(context);
            });
        }
    }
}
