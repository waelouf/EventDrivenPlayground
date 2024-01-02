using Serilog;
using InventoryApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
                .AddJsonFile("Serilog.json")
                .Build();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapInventoryEndpointEndpoints();

app.Run();
Log.CloseAndFlush();