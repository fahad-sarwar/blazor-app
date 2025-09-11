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
        public double? SalePrice { get; set; }
        public List<ProductAttributeViewModel> Attributes { get; set; }
        public double AverageRating { get; set; }

        public string GetFormattedPrice()
        {
            return $"£{Price:0.00}";
        }

        public string GetFormattedSalePrice()
        {
            return $"£{SalePrice.Value:0.00}";
        }

        public string GetStockStatus()
        {
            if (Stock > 10)
                return "In Stock";

            return Stock == 0 ? "Out of Stock" : "Low Stock";
        }

        public string GetStockStatusClass()
        {
            return Stock switch
            {
                0 => "text-danger",
                <= 10 => "text-warning",
                _ => "text-success"
            };
        }
    }
}
