namespace OnlineShopUI.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string CardName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Expiry { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public string MaskedCardNumber =>
            CardNumber.Length > 10
                ? CardNumber.Substring(0, 6) + new string('*', CardNumber.Length - 10) + CardNumber[^4..]
                : CardNumber;
    }
}
