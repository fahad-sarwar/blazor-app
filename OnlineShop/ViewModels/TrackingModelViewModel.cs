using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class TrackingModelViewModel
    {
        [Required(ErrorMessage = "Order number is required")]
        [StringLength(20, ErrorMessage = "Order number cannot exceed 20 characters")]
        public string OrderNumber { get; set; } = string.Empty;
    }
}
