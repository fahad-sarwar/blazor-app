namespace Api.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // e.g. Pending, Approved, Rejected
        public Product Product { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
