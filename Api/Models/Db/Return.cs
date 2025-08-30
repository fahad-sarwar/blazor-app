namespace Api.Models.Db
{
    public class Return
    {
        public int Id { get; set; }
        public string ReturnNumber { get; set; } = string.Empty;
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public string Status { get; set; } = "Requested"; // e.g., Requested, Approved, Shipped, Received, Refunded, Rejected
        public string Comments { get; set; }
        public List<ReturnItem> ReturnItems { get; set; } = [];
        public List<ReturnTrackingUpdate> TrackingUpdates { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
