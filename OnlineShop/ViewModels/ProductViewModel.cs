namespace OnlineShopUI.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public int Stock { get; set; }
        public bool ForSale { get; set; }

        public string GetStockStatus()
        {
            if (Stock > 10)
                return "In Stock";

            return Stock == 0 ? "Out of Stock" : "Low Stock";
        }
    }
}
