using Contracts.Entities;
using Contracts.Events;
using KafkaFlow.Producers;

namespace OrderApi.Services;

public interface IOrdersService
{
    Task<bool> CreateOrder(Order order);
}

public class OrdersService : IOrdersService
{
    private IProducerAccessor _producerAccessor;

    public OrdersService(IProducerAccessor producerAccessor)
    {
        _producerAccessor = producerAccessor;
    }

    public async Task<bool> CreateOrder(Order order)
    {
        // TODO: save order in DB.

        var orderCreated = new OrderCreatedEvent(order.OrderId, 
            order.OrderDate, order.CustomerId, order.ShippingAddress, order.TotalAmount, Common.EventTypes.OrderCreated);

        var producer = _producerAccessor.GetProducer(Common.Kafka.KafkaProducers.PublishOrder);

        await producer.ProduceAsync("key", orderCreated);

        return true;
    }
}
