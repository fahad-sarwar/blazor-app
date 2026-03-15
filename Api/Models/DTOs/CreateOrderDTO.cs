using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class CreateOrderDTO
    {
        [Required]
        public Customer Customer { get; set; }

        [Required]
        public int BasketId { get; set; }

        [Required]
        public PaymentDTO Payment { get; set; }
    }
}
