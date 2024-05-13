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
            consumer.Topic(KafkaTopics.LoggingTopic)
            .WithGroupId(KafkaGroups.LoggingAgg)
            .WithBufferSize(100)
            .WithWorkersCount(3)
            .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Earliest)
            .AddMiddlewares(middlewares => middlewares
                .AddDeserializer<JsonCoreDeserializer>()
                .AddTypedHandlers(handlers =>
                    handlers.AddHandler<LoggerHandler>()
                ));
        });
    }));

var providers = services.BuildServiceProvider();
var bus = providers.CreateKafkaBus();
await bus.StartAsync();

Console.WriteLine("Press key to exit");
Console.ReadKey();
