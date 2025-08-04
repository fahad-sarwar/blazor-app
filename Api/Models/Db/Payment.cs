namespace Api.Models.Db
{
    public class Payment
    {
        public int Id { get; set; }
        public double Amount { get; set; } // Amount paid
        public string PaymentMethod { get; set; } = string.Empty; // e.g., CreditCard, PayPal, etc.
        public string CardName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty; // Encrypted or masked card number
        public string Expiry { get; set; } = string.Empty; // MM/YY format
        public string CVV { get; set; } = string.Empty; // Encrypted or masked CVV
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
