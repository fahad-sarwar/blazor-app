using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class UpdateCustomerDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping address line 1 is required")]
        public string ShippingAddressLineOne { get; set; } = string.Empty;

        public string ShippingAddressLineTwo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping town is required")]
        public string ShippingTown { get; set; } = string.Empty;

        public string ShippingCounty { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping postcode is required")]
        public string ShippingPostCode { get; set; } = string.Empty;

        public string ShippingCountry { get; set; } = "UK";

        [Required(ErrorMessage = "Billing address line 1 is required")]
        public string BillingAddressLineOne { get; set; } = string.Empty;

        public string BillingAddressLineTwo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing town is required")]
        public string BillingTown { get; set; } = string.Empty;

        public string BillingCounty { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing postcode is required")]
        public string BillingPostCode { get; set; } = string.Empty;

        public string BillingCountry { get; set; } = "UK";
    }
}