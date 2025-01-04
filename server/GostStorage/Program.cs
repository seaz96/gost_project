using System.Security.Claims;
using AutoMapper;
using GostStorage.Helpers;
using GostStorage.Middlewares.Extensions;
using GostStorage.Profiles;
using GostStorage.Services.Abstract;
using GostStorage.Services.Concrete;
using GostStorage.StartUp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Serilog;

namespace GostStorage;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var origins = builder.Configuration.GetValue<string>("ORIGINS")!.Split('|');

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
                    ValidIssuer = AuthOptions.AuthTokenIssuer,
                    ValidAudience = AuthOptions.AuthTokenAudience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.SymmetricSecurityKey,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        var mapper = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); })
            .CreateMapper();

        var configuration = builder.Configuration;

        builder.Services.AddSingleton(mapper);
        builder.Services.AddLoggerServices();
        builder.Services.AddInfrastructureServices(configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddSingleton<IPasswordHasher, Sha256PasswordHasher>();
        var sentryService = new SentryService(configuration.GetValue<string>("SENTRY_TOKEN")!, configuration.GetValue<long>("SENTRY_CHAT_ID"));
        builder.Services.AddSingleton<ISentryService>(sentryService);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .WithOrigins(origins)
                        .WithMethods("POST", "GET", "DELETE", "PUT")
                        .AllowAnyHeader()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowCredentials();
                });
        });

        var app = builder.Build();

        app.UseBodyReader();
        app.UseSentry();
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
        app.UseSecurityHeadersComplementary();
        app.MapControllers();

        app.Run();
    }
}