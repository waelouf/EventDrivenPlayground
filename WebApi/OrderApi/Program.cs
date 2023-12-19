using Contracts.Entities;
using KafkaFlow;
using Common.Kafka;
using OrderApi.Services;
using KafkaFlow.Serializer;
using ConfigurationManager = Common.Configuration.ConfigurationManager;
using IConfigurationManager = Common.Configuration.IConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfigurationManager, ConfigurationManager>();
builder.Services.AddSingleton<IOrdersService, OrdersService>();

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
    return Results.Ok(orderCreated);
})
.WithOpenApi();



app.Run();

