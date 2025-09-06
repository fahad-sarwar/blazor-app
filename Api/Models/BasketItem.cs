namespace Api.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice => Quantity * Price;
        public double VATRate { get; set; }
        public double VATAmount => TotalPrice * VATRate;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
