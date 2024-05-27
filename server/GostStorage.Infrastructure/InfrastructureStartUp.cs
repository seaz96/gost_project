using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Persistence;
using GostStorage.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;

namespace GostStorage.Infrastructure;

public static class InfrastructureStartUp
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
        var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        serviceCollection.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") + $"Host={dbHost};" + $"Password={dbPassword};");
        }, ServiceLifetime.Transient);
        
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
        serviceCollection.AddScoped<IFieldsRepository, FieldsRepository>();
        serviceCollection.AddScoped<IReferencesRepository, ReferencesRepository>();
        serviceCollection.AddScoped<IDocsRepository, DocsRepository>();
        serviceCollection.AddScoped<IDocStatisticsRepository, DocStatisticsRepository>();
        serviceCollection.AddMinio(new Uri("s3://EV9fjF0qebstRDG6qzrK:QHFGTdz0p4qiRhPcQjLTpeZZuJONpn3bs8c9guHh@localhost:9000/"));
        serviceCollection.AddScoped<IFilesRepository, FilesRepository>();
        
        return serviceCollection;
    }
}