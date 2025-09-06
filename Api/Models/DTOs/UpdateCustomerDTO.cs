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
        public UpdateAddressDTO ShippingAddress { get; set; } = new();

        [Required]
        public UpdateAddressDTO BillingAddress { get; set; } = new();
    }
}