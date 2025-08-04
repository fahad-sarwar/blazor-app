namespace Api.Models
{
    public class CreateCustomerRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public CreateAddressRequest ShippingAddress { get; set; } = new();
        public CreateAddressRequest BillingAddress { get; set; } = new();
    }
}
