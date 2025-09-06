namespace Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Customer Customer { get; set; } = new Customer();
        public double TotalPrice { get; set; }
        public double VATRate { get; set; }
        public double VATAmount => TotalPrice * VATRate;
        public List<OrderItem> OrderItems { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Cancelled
        public Payment Payment { get; set; } = new Payment();
        public string DeliveryMethod { get; set; } = string.Empty;
        public DateTime? EstimatedDelivery { get; set; }
        public List<OrderTrackingUpdate> TrackingUpdates { get; set; } = new List<OrderTrackingUpdate>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
