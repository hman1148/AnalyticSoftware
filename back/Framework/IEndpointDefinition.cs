using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface IEndpointDefinition
    {
        void DefineEndpoint(IEndpointRouteBuilder app);
    }
}
