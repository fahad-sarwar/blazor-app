using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs
{
    public class UpdateAddressDTO
    {
        [Required]
        public int Id { get; set; } 

        [Required]
        public string AddressLineOne { get; set; } = string.Empty;

        public string AddressLineTwo { get; set; } = string.Empty;

        [Required]
        public string Town { get; set; } = string.Empty;

        public string County { get; set; } = string.Empty;

        [Required]
        public string PostCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = "UK";
    }
}
