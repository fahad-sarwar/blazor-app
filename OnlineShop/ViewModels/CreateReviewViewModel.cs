namespace OnlineShopUI.ViewModels
{
    public class CreateReviewViewModel
    {
        public string Subject { get; set; } = string.Empty;
        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
