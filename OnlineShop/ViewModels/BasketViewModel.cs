namespace OnlineShopUI.ViewModels
{
    public class BasketViewModel
    {
        public int Id { get; set; }
        public string? AnonymousId { get; set; }
        public List<BasketItemViewModel> Items { get; set; } = new();

        public double Subtotal => Items.Sum(item => item.LineTotal);
        public double Vat => Subtotal * 0.20;
        public double Total => Subtotal + Vat;
    }
}
