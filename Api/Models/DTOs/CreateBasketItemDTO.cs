using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class CreateBasketItemDTO
    {
        public int? CustomerId { get; set; }
        public string? AnonymousId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
