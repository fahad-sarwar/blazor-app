namespace Api.Models
{
    public class CreatePaymentRequest
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardName { get; set; } = string.Empty;
        public string Expiry { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
    }
}
