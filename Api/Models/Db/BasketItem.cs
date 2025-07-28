namespace Api.Models.Db
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int BasketId { get; set; } // Foreign key to Basket
        public Product Product { get; set; } // Navigation property to Product
        public int Quantity { get; set; }
        public double Price { get; set; } // Price at the time of adding to the basket
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
