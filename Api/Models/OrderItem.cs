namespace Api.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public double VATRate { get; set; }
        public double VATAmount => TotalPrice * VATRate;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
