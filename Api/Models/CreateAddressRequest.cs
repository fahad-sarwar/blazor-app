namespace Api.Models
{
    public class CreateAddressRequest
    {
        public string AddressLineOne { get; set; } = string.Empty;
        public string AddressLineTwo { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public string Country { get; set; } = "UK";
    }
}
