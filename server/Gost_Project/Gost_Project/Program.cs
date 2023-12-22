using AutoMapper;
using Gost_Project.Data;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Profiles;
using Gost_Project.Services.Abstract;
using Gost_Project.Services.Concrete;
using Microsoft.EntityFrameworkCore;

class Porgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var mapperConfig = new MapperConfiguration(config => config.AddProfile(new MapperProfile()));
        var mapper = mapperConfig.CreateMapper();

        builder.Services.AddSingleton(mapper);

        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IFieldsRepository, FieldsRepository>();
        builder.Services.AddScoped<IReferencesRepository, ReferencesRepository>();
        builder.Services.AddScoped<IDocsRepository, DocsRepository>();
        builder.Services.AddScoped<IDocsService, DocsService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}