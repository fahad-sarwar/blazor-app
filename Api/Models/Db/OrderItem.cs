namespace Api.Models.Db
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Foreign key to Order
        public Product Product { get; set; } // Navigation property to Product
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public double VATRate { get; set; } // Stored for audit/history
        public double VATAmount => TotalPrice * VATRate;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
