namespace OnlineShopUI.ViewModels
{
    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public double LineTotal => Quantity * Price;
    }
}
