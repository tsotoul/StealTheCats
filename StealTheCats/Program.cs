using Microsoft.EntityFrameworkCore;
using StealTheCatsApi;
using StealTheCatsApi.Configuration;
using StealTheCatsApi.Mappings;
using StealTheCatsApi.Repositories;
using StealTheCatsApi.Repositories.Interfaces;
using StealTheCatsApi.Services;
using StealTheCatsApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddTransient<ICatsService, CatsService>();
builder.Services.AddTransient<ICatsApiRepository, CatsApiRepository>();
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Cat_Api_Settings"));
builder.Services.AddAutoMapper(typeof(MapProfileDtoToDomain));
builder.Services.AddDbContext<CatsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatsDatabaseConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Steal The Cats API";
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
