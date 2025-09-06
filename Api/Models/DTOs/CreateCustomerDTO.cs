using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class CreateCustomerDTO
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public CreateAddressDTO ShippingAddress { get; set; } = new();

        [Required]
        public CreateAddressDTO BillingAddress { get; set; } = new();
    }
}
