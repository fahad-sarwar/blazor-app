using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class PaymentDTO
    {
        [Required]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        public string CardName { get; set; } = string.Empty;

        [Required]
        public string Expiry { get; set; } = string.Empty;

        [Required]
        public string CVV { get; set; } = string.Empty;
    }
}
