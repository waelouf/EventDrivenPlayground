namespace Contracts.Events;

public record OrderCreatedEvent(int OrderId, 
    DateTime OrderDate, 
    string CustomerId, 
    string ShippingAddress, 
    decimal TotalAmount, 
    string EventType);
