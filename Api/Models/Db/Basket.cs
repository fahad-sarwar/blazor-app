namespace Api.Models.Db
{
    public class Basket
    {
        public int Id { get; set; }
        public Customer? Customer { get; set; }
        public string? AnonymousId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<BasketItem> Items { get; set; } = new();
    }
}
