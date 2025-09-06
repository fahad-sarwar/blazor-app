using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class AddToWishListDTO
    {
        [Required]
        public int ProductId { get; set; }
    }
}
