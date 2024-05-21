using GostStorage.Services.Services.Abstract;
using GostStorage.Services.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;

namespace GostStorage.Services;

public static class ServicesStartUp
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<IDocsService, DocsService>();
        serviceCollection.AddScoped<IReferencesService, ReferencesService>();
        serviceCollection.AddScoped<IFieldsService, FieldsService>();
        serviceCollection.AddScoped<IDocStatisticsService, DocStatisticsService>();
        serviceCollection.AddMinio(new Uri("s3://EV9fjF0qebstRDG6qzrK:QHFGTdz0p4qiRhPcQjLTpeZZuJONpn3bs8c9guHh@localhost:9000/"));
        
        return serviceCollection;
    }
}