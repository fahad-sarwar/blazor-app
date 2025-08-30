namespace OnlineShopUI.ViewModels
{
    public class PagedOrderResultViewModel
    {
        public List<OrderViewModel> Orders { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
