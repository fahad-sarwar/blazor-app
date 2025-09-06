using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class UpdateCustomerViewModel
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

        public UpdateAddressViewModel ShippingAddress { get; set; }
        public UpdateAddressViewModel BillingAddress { get; set; }
    }
}
