using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class CreateReviewDTO
    {
        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public int Rating { get; set; } // 1 to 5

        [Required]
        public string Comment { get; set; } = string.Empty;

        [Required]
        public int ProductId { get; set; }
    }
}
