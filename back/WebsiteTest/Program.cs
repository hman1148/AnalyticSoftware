using AnalyticSoftware.Database;
using AnalyticSoftware.Services;
using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.IdentityModel.Tokens;
using AnalyticSoftware.MiddleWare;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

MongoDBSettings mongoDBSettings = new MongoDBSettings
{
    ConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION") ?? "",
    DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? ""
};

// Add logging
builder.Logging.AddConsole();
string jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey))
    };
});

//Add Services
builder.Services.AddAuthorization();
builder.Services.AddSingleton(mongoDBSettings);
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<S3Service>(sp => new S3Service("access_key", "secret_key", "region"));
builder.Services.AddSingleton<SecurityService>();

var endpointDefinitionTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpointDefinition).IsAssignableFrom(t))
    .ToList();

foreach (var endpointDefinitionType in endpointDefinitionTypes)
{
    builder.Services.AddSingleton(typeof(IEndpointDefinition), endpointDefinitionType);
}

var app = builder.Build();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

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
