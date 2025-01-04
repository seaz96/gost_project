using GostStorage.Services.Abstract;
using GostStorage.Services.Concrete;

namespace GostStorage.StartUp;

public static class ServicesStartUp
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<IDocumentsService, DocumentsService>();
        serviceCollection.AddScoped<IReferencesService, ReferencesService>();
        serviceCollection.AddScoped<IFieldsService, FieldsService>();
        serviceCollection.AddScoped<IUserActionsService, UserActionsService>();
    }
}