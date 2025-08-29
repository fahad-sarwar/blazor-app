namespace OnlineShopUI.ViewModels
{
    public class PagedReviewResultViewModel
    {
        public List<ReviewViewModel> Reviews { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
