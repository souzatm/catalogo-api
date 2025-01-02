using ApiCatalogo.Data;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Ajuste de refer�ncia c�clica
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
    .AddJsonOptions(
    options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ApiLoggingFilter>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviredConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
