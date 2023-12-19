namespace Contracts.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string CustomerId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public string ShippingAddress { get; set; }

        public decimal TotalAmount { get; set; }

        // Additional properties can be added based on specific requirements

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
