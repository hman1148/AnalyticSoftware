using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.AddConsole();

var endpointDefinitionTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpointDefinition).IsAssignableFrom(t))
    .ToList();

foreach (var endpointDefinitionType in endpointDefinitionTypes)
{
    builder.Services.AddSingleton(typeof(IEndpointDefinition), endpointDefinitionType);
}

var app = builder.Build();

app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var endpointDefinitions = scope.ServiceProvider.GetServices<IEndpointDefinition>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    foreach (var endpointDef in endpointDefinitions)
    {
        endpointDef.DefineEndpoint(app);
    }
}

app.MapGet("/", () => "Hello World!");

app.Run();
