// See https://aka.ms/new-console-template for more information

using Common.Kafka;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(config => config.AddConsole());

var configMan = new Common.Configuration.ConfigurationManager();
services.AddKafkaFlowHostedService(
    kafka => kafka
    .UseMicrosoftLog()
    .AddCluster(cluster =>
    {
        cluster.WithBrokers(new[] { $"{configMan.GetEnvironmentVariable("KafkaClusterIP")}:9092" })
        .AddConsumer(consumer =>
        {
            consumer.Topic(KafkaTopics.OrdersTopic)
            .WithGroupId(KafkaGroups.Notifications)
            .WithBufferSize(100)
            .WithWorkersCount(3)
            .WithAutoOffsetReset(AutoOffsetReset.Earliest)
            .AddMiddlewares(middlewares => middlewares
                .AddDeserializer<JsonCoreDeserializer>()
                .AddTypedHandlers(handlers =>
                    handlers.AddHandler<OrderCreatedEventHandler>()
                ));
        });
    }));

var providers = services.BuildServiceProvider();
var bus = providers.CreateKafkaBus();
await bus.StartAsync();

Console.WriteLine("Press key to exit");
Console.ReadKey();
