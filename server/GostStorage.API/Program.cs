using AutoMapper;
using GostStorage.API.Helpers;
using GostStorage.API.Middlewares.Extensions;
using GostStorage.Infrastructure;
using GostStorage.Services;
using GostStorage.Services.Profiles;
using GostStorage.Services.Services.Abstract;
using GostStorage.Services.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;

namespace GostStorage.API;

static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var securityKey = builder.Configuration.GetSection("Security")["SecurityKey"];

        ArgumentNullException.ThrowIfNull(securityKey);
        AuthOptions.Initialize(securityKey);

        builder.Host.UseSerilog((ctx, lc) => lc.GetConfiguration());

        builder.Services.AddControllers();
        builder.Services.AddAuthentication();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = AuthOptions.AUTH_TOKEN_ISSUER,
                    ValidAudience = AuthOptions.AUTH_TOKEN_AUDIENCE,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.SymmetricSecurityKey,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new MapperProfile());
        })
        .CreateMapper();

        builder.Services.AddSingleton(mapper);
        builder.Services.AddLoggerServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddTransient<IPasswordHasher, Sha256PasswordHasher>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:3000", "https://gost-storage.ru")
                        .WithMethods("POST", "GET", "DELETE", "PUT")
                        .AllowAnyHeader()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowCredentials();
                });
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            await next.Invoke();
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSecurityHeadersComplementaryMiddleware();
        app.UseSessionValidityCheckMiddlewareMiddleware();
        app.UseAuthTokenRefreshMiddleware();
        app.Use(async (context, next) =>
        {
            if (context.Request.Method == "POST")
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
                var body = await reader.ReadToEndAsync();
                Console.WriteLine("HTTP request body: " + body);
                var byteArray = Encoding.UTF8.GetBytes(body);
                var stream = new MemoryStream(byteArray);
                context.Request.Body = stream;
            }
            await next.Invoke();
        });
        app.MapControllers();

        app.Run();
    }
}