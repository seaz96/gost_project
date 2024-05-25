using GostStorage.Services.Services.Abstract;
using GostStorage.Services.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

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

        return serviceCollection;
    }
}