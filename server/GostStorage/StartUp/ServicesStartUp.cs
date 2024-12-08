using GostStorage.Services;
using GostStorage.Services.Concrete;
using GostStorage.Services.Interfaces;

namespace GostStorage.StartUp;

public static class ServicesStartUp
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<IDocsService, DocsService>();
        serviceCollection.AddScoped<IReferencesService, ReferencesService>();
        serviceCollection.AddScoped<IFieldsService, FieldsService>();
        serviceCollection.AddScoped<IDocStatisticsService, DocStatisticsService>();
    }
}