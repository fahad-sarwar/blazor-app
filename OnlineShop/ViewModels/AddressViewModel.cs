using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class AddressViewModel
    {
        [Required(ErrorMessage = "Address Line 1 is required")]
        public string AddressLineOne { get; set; } = string.Empty;

        public string AddressLineTwo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Town is required")]
        public string Town { get; set; } = string.Empty;

        public string County { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postcode is required")]
        public string PostCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; } = "UK";
    }
}
