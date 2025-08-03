﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class CustomerViewModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public AddressViewModel ShippingAddress { get; set; } = new();

        [Required]
        public AddressViewModel BillingAddress { get; set; } = new();
    }
}
