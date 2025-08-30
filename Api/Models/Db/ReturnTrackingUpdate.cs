namespace Api.Models.Db
{
    public class ReturnTrackingUpdate
    {
        public int Id { get; set; }
        public int ReturnId { get; set; } // Foreign key to the Return
        public string UpdatedBy { get; set; } = string.Empty; // User or system that made the update
        public string Status { get; set; } = "Requested"; // e.g., Requested, Approved, Shipped, Received, Refunded, Rejected
        public string Note { get; set; } = string.Empty; // Optional note for the update
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
