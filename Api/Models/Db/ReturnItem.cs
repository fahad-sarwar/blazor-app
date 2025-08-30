namespace Api.Models.Db
{
    public class ReturnItem
    {
        public int Id { get; set; }
        public int ReturnId { get; set; } // Foreign key to Return
        public Product Product { get; set; } // Navigation property to Product
        public OrderItem OrderItem { get; set; }
        public int Quantity { get; set; }
        public string ReturnReason { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public double VATRate { get; set; } // Stored for audit/history
        public double VATAmount => TotalPrice * VATRate;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
