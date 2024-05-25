using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Persistence;
using GostStorage.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GostStorage.Infrastructure;

public static class InfrastructureStartUp
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
        var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

        serviceCollection.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }, ServiceLifetime.Transient);

        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
        serviceCollection.AddScoped<IFieldsRepository, FieldsRepository>();
        serviceCollection.AddScoped<IReferencesRepository, ReferencesRepository>();
        serviceCollection.AddScoped<IDocsRepository, DocsRepository>();
        serviceCollection.AddScoped<IDocStatisticsRepository, DocStatisticsRepository>();
        serviceCollection.AddScoped<IUserSessionsRepository, UsersSessionsRepository>();

        return serviceCollection;
    }
}