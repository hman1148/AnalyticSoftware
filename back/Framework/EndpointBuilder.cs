using Microsoft.AspNetCore.Http;

namespace Framework
{
    public class Endpointbuilder
    {
        private readonly Endpoint _endpoint;

        public Endpointbuilder()
        {
            _endpoint = new Endpoint();
        }

        public Endpointbuilder WithRoute(string route)
        {
            _endpoint.Route = route;    
            return this;        
        }

        public Endpointbuilder WithHttpMethod(HttpMethod method)
        {
            _endpoint.HttpMethod = method;
            return this;
        }

        public Endpointbuilder WithAction(Func<HttpContext, Task> action)
        {
            _endpoint.Action = action;
            return this;
        }

        public Endpoint Build()
        {
            return _endpoint;
        }
    }
}
