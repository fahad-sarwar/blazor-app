namespace Api.Models
{
    public class OrderTrackingUpdate
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Completed, Cancelled
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
