using StealTheCats.Configuration;
using StealTheCats.Mappings;
using StealTheCats.Repositories;
using StealTheCats.Repositories.Interfaces;
using StealTheCats.Services;
using StealTheCats.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddTransient<ICatsService, CatsService>();
builder.Services.AddTransient<ICatsApiRepository, CatsApiRepository>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Cat_Api_Settings"));
builder.Services.AddAutoMapper(typeof(MapProfileDtoToDomain));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Steal The Cats API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
