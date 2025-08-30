using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class ReturnRequestViewModel
    {
        [Required(ErrorMessage = "Order number is required")]
        [StringLength(20, ErrorMessage = "Order number cannot exceed 20 characters")]
        public string OrderNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters")]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a reason for return")]
        public string Reason { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string Comments { get; set; } = string.Empty;
    }
}