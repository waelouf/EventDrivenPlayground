using Contracts.Entities;
using KafkaFlow;
using Common.Kafka;
using OrderApi.Services;
using KafkaFlow.Serializer;
using ConfigurationManager = Common.Configuration.ConfigurationManager;
using IConfigurationManager = Common.Configuration.IConfigurationManager;
using Serilog;
using Serilog.Sinks.Kafka;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();


builder.Services.AddSingleton<IConfigurationManager, ConfigurationManager>();
builder.Services.AddSingleton<IOrdersService, OrdersService>();
builder.Host.UseSerilog();

builder.Services.AddKafka(
    kafka => kafka
    .AddCluster(cluster =>
    {
        cluster.WithBrokers(new[] { "localhost:9092" })
        .CreateTopicIfNotExists(KafkaTopics.OrdersTopic, 1, 1)
        .AddProducer(KafkaProducers.PublishOrder, producer =>
            producer
            .DefaultTopic(KafkaTopics.OrdersTopic)
            .AddMiddlewares(middlewares => middlewares.AddSerializer<JsonCoreSerializer>())
        );
    })
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/", (IOrdersService service, Order order) =>
{
    var orderCreated = service.CreateOrder(order);

    for (int i = 0; i < 1_000; i++)
    {
        Log.Warning("Warning @{i}", i);
    }

    return Results.Ok(orderCreated);
})
.WithOpenApi();



app.Run();
