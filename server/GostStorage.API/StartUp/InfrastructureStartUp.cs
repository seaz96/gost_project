using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using GostStorage.API.Data;
using GostStorage.API.Repositories.Concrete;
using GostStorage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Minio.AspNetCore;

namespace GostStorage.API.StartUp;

public static class InfrastructureStartUp
{
    public static void AddInfrastructureServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        // postgresql
        var dbHost = configuration.GetValue<string>("DATABASE_HOST");
        var dbPassword = configuration.GetValue<string>("DATABASE_PASSWORD");
        var dbName = configuration.GetValue<string>("DATABASE_NAME");
        var dbUser = configuration.GetValue<string>("DATABASE_USER");
        var dbPort = configuration.GetValue<string>("DATABASE_PORT");
        
        // minio s3
        var minioPublicKey = configuration.GetValue<string>("MINIO_PUBLIC_KEY");
        var minioPrivateKey = configuration.GetValue<string>("MINIO_PRIVATE_KEY");
        
        // elastic
        var elasticPassword = configuration.GetValue<string>("ELASTIC_PASSWORD");
        var elasticIndex = configuration.GetValue<string>("ELASTIC_INDEX");
        var elasticHost = configuration.GetValue<string>("ELASTIC_HOST");
        var elasticSettings = new ElasticsearchClientSettings(new Uri(elasticHost!))
            .Authentication(new BasicAuthentication("elastic", elasticPassword!))
            .DefaultIndex(elasticIndex!);
        
        serviceCollection.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql($"Port={dbPort}; Database={dbName}; Username={dbUser}; Host={dbHost}; Password={dbPassword};");
        }, ServiceLifetime.Transient);
        
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
        serviceCollection.AddScoped<IFieldsRepository, FieldsRepository>();
        serviceCollection.AddScoped<IReferencesRepository, ReferencesRepository>();
        serviceCollection.AddScoped<IDocsRepository, DocsRepository>();
        serviceCollection.AddScoped<IDocStatisticsRepository, DocStatisticsRepository>();
        serviceCollection.AddMinio(new Uri($"s3://{minioPublicKey}:{minioPrivateKey}@minio:9000/"));
        serviceCollection.AddScoped<ElasticsearchClient>(_ => new ElasticsearchClient(elasticSettings));
        serviceCollection.AddScoped<IFilesRepository, FilesRepository>();
        serviceCollection.AddScoped<ISearchRepository, SearchRepository>();
    }
}