using Anomaly.Middlewares.Extensions;
using AutoMapper;
using CorsairMessengerServer;
using Gost_Project.Data;
using Gost_Project.Data.Entities.Navigations;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Profiles;
using Gost_Project.Services.Abstract;
using Gost_Project.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var securityKey = builder.Configuration.GetSection("Security")["SecurityKey"];

        ArgumentNullException.ThrowIfNull(securityKey);
        AuthOptions.Initialize(securityKey);

        builder.Services.AddControllers();
        builder.Services.AddAuthentication();

        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

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

        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IFieldsRepository, FieldsRepository>();
        builder.Services.AddScoped<IReferencesRepository, ReferencesRepository>();
        builder.Services.AddScoped<IDocsRepository, DocsRepository>();
        builder.Services.AddScoped<IDocStatisticsRepository, DocStatisticsRepository>();
        
        builder.Services.AddScoped<IDocsService, DocsService>();
        builder.Services.AddScoped<IReferencesService, ReferencesService>();
        builder.Services.AddScoped<IFieldsService, FieldsService>();
        builder.Services.AddScoped<IDocStatisticsService, DocStatisticsService>();
        
        builder.Services.AddTransient<IPasswordHasher, Sha256PasswordHasher>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Gost_project - V1",
                    Version = "v1"
                }
            );

            var filePath = Path.Combine(System.AppContext.BaseDirectory, "Gost_Project.xml");
            c.IncludeXmlComments(filePath);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseHttpsRedirection();
        app.UseRequestHeadersComplementaryMiddleware();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSecurityHeadersComplementaryMiddleware();
        app.MapControllers();

        app.Run();
    }
}