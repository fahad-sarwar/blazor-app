using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class UpdateCustomerDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string ShippingAddressLineOne { get; set; } = string.Empty;

        public string ShippingAddressLineTwo { get; set; } = string.Empty;

        [Required]
        public string ShippingTown { get; set; } = string.Empty;

        public string ShippingCounty { get; set; } = string.Empty;

        [Required]
        public string ShippingPostCode { get; set; } = string.Empty;

        public string ShippingCountry { get; set; } = "UK";

        [Required]
        public string BillingAddressLineOne { get; set; } = string.Empty;

        public string BillingAddressLineTwo { get; set; } = string.Empty;

        [Required]
        public string BillingTown { get; set; } = string.Empty;

        public string BillingCounty { get; set; } = string.Empty;

        [Required]
        public string BillingPostCode { get; set; } = string.Empty;

        public string BillingCountry { get; set; } = "UK";
    }
}