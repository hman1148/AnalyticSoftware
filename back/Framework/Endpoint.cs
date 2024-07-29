using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Endpoint
    {
        public string Route { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public Func<HttpContext, Task> Action { get; set; }
    }
}
