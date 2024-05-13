using Contracts.Entities;
using KafkaFlow;
using Common.Kafka;
using OrderApi.Services;
using KafkaFlow.Serializer;
using ConfigurationManager = Common.Configuration.ConfigurationManager;
using IConfigurationManager = Common.Configuration.IConfigurationManager;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
                .AddJsonFile("Serilog.json", optional: false, reloadOnChange: true)
                .Build();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();


builder.Services.AddSingleton<IConfigurationManager, ConfigurationManager>();
builder.Services.AddSingleton<IOrdersService, OrdersService>();

builder.Host.UseSerilog();

var configMan = new ConfigurationManager();

builder.Services.AddKafka(
    kafka => kafka
    .AddCluster(cluster =>
    {
        cluster.WithBrokers(new[] { $"{configMan.GetEnvironmentVariable("KafkaClusterIP")}:9092" })
        .CreateTopicIfNotExists(KafkaTopics.OrdersTopic, 1, 1)
        .AddProducer(KafkaProducers.PublishOrder, producer =>
            producer
            .DefaultTopic(KafkaTopics.OrdersTopic)
            .AddMiddlewares(middlewares => middlewares.AddSerializer<NewtonsoftJsonSerializer>())
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
    for (int i = 0; i < 1_000; i++)
    {
        Log.Warning("Warning {@i}", i);
    }

    var orderCreated = service.CreateOrder(order);
    
    return Results.Ok(orderCreated);
})
.WithOpenApi();



app.Run();
Log.CloseAndFlush();

