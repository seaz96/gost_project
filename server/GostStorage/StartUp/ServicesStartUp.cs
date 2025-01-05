using GostStorage.Services.Abstract;
using GostStorage.Services.Concrete;
using java.awt.@event;
using TikaOnDotNet.TextExtraction;

namespace GostStorage.StartUp;

public static class ServicesStartUp
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<TextExtractor>();
        serviceCollection.AddScoped<IFileProcessor, FileProcessor>();
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<IDocumentsService, DocumentsService>();
        serviceCollection.AddScoped<IReferencesService, ReferencesService>();
        serviceCollection.AddScoped<IFieldsService, FieldsService>();
        serviceCollection.AddScoped<IUserActionsService, UserActionsService>();
    }
}