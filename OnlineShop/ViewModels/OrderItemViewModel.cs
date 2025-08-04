namespace OnlineShopUI.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
