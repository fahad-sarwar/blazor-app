namespace Api.Models.Db
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } // Navigation property
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp for ordering
    }
}
