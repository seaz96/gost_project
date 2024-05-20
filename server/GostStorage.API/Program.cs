using System.Text;
using AutoMapper;
using GostStorage.API.Helpers;
using GostStorage.API.Middlewares.Extensions;
using GostStorage.Infrastructure;
using GostStorage.Services;
using GostStorage.Services.Helpers;
using GostStorage.Services.Profiles;
using GostStorage.Services.Services.Abstract;
using GostStorage.Services.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

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
        app.Use(async (context, next) =>
        {
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                Console.WriteLine("HTTP request body: " + await reader.ReadToEndAsync());
            }
            await next.Invoke();
        });
        app.MapControllers();

        app.Run();
    }
}