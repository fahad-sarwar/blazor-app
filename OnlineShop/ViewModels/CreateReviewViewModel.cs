using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class CreateReviewViewModel
    {
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = string.Empty;

        public int Rating { get; set; } // 1 to 5

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; } = string.Empty;

        public int ProductId { get; set; }
    }
}
