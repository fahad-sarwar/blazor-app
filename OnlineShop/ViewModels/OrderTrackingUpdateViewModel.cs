namespace OnlineShopUI.ViewModels
{
    public class OrderTrackingUpdateViewModel
    {
        public string UpdatedBy { get; set; } = string.Empty; // User or system that made the update
        public string Status { get; set; } = "Pending"; // e.g., Pending, Processing, Shipped, Completed, Cancelled
        public string Note { get; set; } = string.Empty; // Optional note for the update
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
