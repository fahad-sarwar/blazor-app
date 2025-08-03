using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;



        [Required(ErrorMessage = "Billing Address Line 1 is required")]
        public string BillingAddressLineOne { get; set; } = string.Empty;

        public string BillingAddressLineTwo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing Town is required")]
        public string BillingTown { get; set; } = string.Empty;

        public string BillingCounty { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing Postcode is required")]
        public string BillingPostCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing Country is required")]
        public string BillingCountry { get; set; } = "UK";



        [Required(ErrorMessage = "Shipping Address Line 1 is required")]
        public string ShippingAddressLineOne { get; set; } = string.Empty;

        public string ShippingAddressLineTwo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping Town is required")]
        public string ShippingTown { get; set; } = string.Empty;

        public string ShippingCounty { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping Postcode is required")]
        public string ShippingPostCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping Country is required")]
        public string ShippingCountry { get; set; } = "UK";



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
