namespace Api.Models
{
    public class AddBasketItem
    {
        public int? CustomerId { get; set; }
        public string? AnonymousId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
