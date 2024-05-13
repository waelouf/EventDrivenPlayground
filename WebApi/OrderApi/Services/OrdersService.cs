using Contracts.Entities;
using Contracts.Events;
using KafkaFlow.Producers;

namespace OrderApi.Services;

public interface IOrdersService
{
    bool CreateOrder(Order order);
}

public class OrdersService : IOrdersService
{
    private IProducerAccessor _producerAccessor;
    private ILogger<OrdersService> _logger;

    public OrdersService(IProducerAccessor producerAccessor, ILogger<OrdersService> logger)
    {
        _producerAccessor = producerAccessor;
        this._logger = logger;
    }

    public bool CreateOrder(Order order)
    {
        // TODO: save order in DB.

        var orderCreated = new OrderCreatedEvent(order.OrderId, 
            order.OrderDate, order.CustomerId, order.ShippingAddress, order.TotalAmount, Common.EventTypes.OrderCreated);

        var producer = _producerAccessor.GetProducer(Common.Kafka.KafkaProducers.PublishOrder);

        try
        {
            producer.ProduceAsync("key", orderCreated).Wait();
        }
        catch (Exception ex)
        {
            this._logger.LogError("Producer publising failed with message {@message}", ex.Message, ex);
        }
        

        return true;
    }
}
