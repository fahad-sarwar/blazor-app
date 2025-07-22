namespace OnlineShopUI.ViewModels
{
    public class PagedProductResultViewModel
    {
        public List<ProductViewModel> Products { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
