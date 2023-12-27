// See https://aka.ms/new-console-template for more information

using Contracts.Events;
using KafkaFlow;
using Microsoft.Extensions.Logging;

public class OrderCreatedEventHandler : IMessageHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IMessageContext context, OrderCreatedEvent message)
    {
        _logger.LogInformation("New order {OrderId} created", message.OrderId);

        return Task.CompletedTask;
    }
}