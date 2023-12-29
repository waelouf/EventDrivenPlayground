using KafkaFlow;
using Microsoft.Extensions.Logging;

public class LoggerHandler : IMessageHandler<string>
{
    private readonly ILogger<LoggerHandler> _logger;

    public LoggerHandler(ILogger<LoggerHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IMessageContext context, string message)
    {
        _logger.LogInformation(message);

        return Task.CompletedTask;
    }
}
