using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class PaymentDetails
    {
        [Required(ErrorMessage = "Card number is required")]
        [CreditCard(ErrorMessage = "Invalid card number")]
        public string CardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name on card is required")]
        public string CardName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expiry date is required")]
        [RegularExpression("^(0[1-9]|1[0-2])\\/([0-9]{2})$", ErrorMessage = "Expiry must be in MM/YY format")]
        public string Expiry { get; set; } = string.Empty;

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression("^[0-9]{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
        public string CVV { get; set; } = string.Empty;
    }
}
