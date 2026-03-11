using Api.Models;

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
        public List<ProductAttribute> Attributes { get; set; }
        public double AverageRating { get; set; }

        public string FormattedAverageRating()
        {
            return AverageRating.ToString("0.0");
        }

        public string FormattedPrice()
        {
            return Price.ToString("C");
        }

        public string FormattedSalePrice()
        {
            return SalePrice.HasValue
                ? SalePrice.Value.ToString("C")
                : string.Empty;
        }

        public string StockStatus()
        {
            if (Stock > 10)
                return "In Stock";

            return Stock == 0 ? "Out of Stock" : "Low Stock";
        }

        public string StockStatusClass()
        {
            switch (Stock)
            {
                case 0:
                    return "text-danger";
                case int n when n <= 10:
                    return "text-warning";
                default:
                    return "text-success";
            }
        }
    }
}
