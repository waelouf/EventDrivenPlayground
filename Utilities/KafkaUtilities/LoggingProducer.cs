using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using Common.Kafka;
namespace KafkaUtilities;

public static class LoggingProducer
{
    private static void AddLoggingCluster(this IServiceCollection services)
    {
        services.AddKafka(
            kafka => kafka
            .AddCluster(cluster =>
            cluster.WithBrokers(new[] { "localhost:9092" })
            .CreateTopicIfNotExists(Common.Kafka.KafkaTopics.LoggingTopic, 1, 1)
            .AddProducer(KafkaProducers.PublishLogs, producer =>
            producer.DefaultTopic(KafkaTopics.LoggingTopic))
                )
            );
    }
}
