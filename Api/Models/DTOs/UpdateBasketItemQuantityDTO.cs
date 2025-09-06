using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class UpdateBasketItemQuantityDTO
    {
        [Required]
        public int Quantity { get; set; }
    }
}
