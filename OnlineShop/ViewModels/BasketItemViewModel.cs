namespace OnlineShopUI.ViewModels
{
    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double VATRate { get; set; }

        public string FormattedPrice()
        {
            return Price.ToString("C");
        }

        public double LineTotal()
        {
            return Quantity * Price;
        }

        public string FormattedLineTotal()
        {
            return LineTotal().ToString("C");
        }
    }
}
