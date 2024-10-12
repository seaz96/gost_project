using GostStorage.API.Data;
using GostStorage.API.Repositories.Concrete;
using GostStorage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Minio.AspNetCore;

namespace GostStorage.API.StartUp;

public static class InfrastructureStartUp
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
        var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        var minioPublicKey = Environment.GetEnvironmentVariable("MINIO_PUBLIC_KEY");
        var minioPrivateKey = Environment.GetEnvironmentVariable("MINIO_PRIVATE_KEY");
        serviceCollection.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") + $"Host={dbHost};" + $"Password={dbPassword};");
        }, ServiceLifetime.Transient);
        
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
        serviceCollection.AddScoped<IFieldsRepository, FieldsRepository>();
        serviceCollection.AddScoped<IReferencesRepository, ReferencesRepository>();
        serviceCollection.AddScoped<IDocsRepository, DocsRepository>();
        serviceCollection.AddScoped<IDocStatisticsRepository, DocStatisticsRepository>();
        serviceCollection.AddMinio(new Uri($"s3://{minioPublicKey}:{minioPrivateKey}@minio:9000/"));
        serviceCollection.AddScoped<IFilesRepository, FilesRepository>();
        serviceCollection.AddScoped<ISearchRepository, SearchRepository>();
        
        return serviceCollection;
    }
}