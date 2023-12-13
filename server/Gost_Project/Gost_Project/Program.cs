using AutoMapper;
using Gost_Project.Data;
using Gost_Project.Data.Repositories.Fields;
using Gost_Project.Data.Repositories.Gosts;
using Gost_Project.Data.Repositories.References;
using Gost_Project.Data.Repositories.Users;
using Gost_Project.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"));
});

var mapperConfig = new MapperConfiguration(config => config.AddProfile(new MapperProfile()));
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IFieldsRepository, FieldsRepository>();
builder.Services.AddScoped<IReferencesRepository, ReferencesRepository>();
builder.Services.AddScoped<IGostsRepository, GostsRepository>();

builder.Services.AddScoped<IGostsService, GostsService>();

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