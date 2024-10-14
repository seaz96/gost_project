using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace GostStorage.Helpers;

public static class SwaggerHelper
{
    public static IServiceCollection AddSwaggerGen(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Gost_project - V1",
                    Version = "v1"
                }
            );

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });

#if DEBUG
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "GostStorage.xml");
#else
            var filePath = Path.Combine(AppContext.BaseDirectory, "GostStorage.xml");
#endif

            c.IncludeXmlComments(filePath);
        });

        return serviceCollection;
    }
}