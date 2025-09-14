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

        public string MaskedCardNumber()
        {
            if(string.IsNullOrEmpty(CardNumber))
                return "-";

            if(CardNumber.Length < 10)
                return new string('*', CardNumber.Length);

            var firstSix = CardNumber.Substring(0, 6);
            var maskedSection = new string('*', CardNumber.Length - 10);
            var lastFour = CardNumber.Substring(CardNumber.Length - 4, 4);

            return firstSix + maskedSection + lastFour;
        }
    }
}
