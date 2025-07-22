namespace OnlineShopUI.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; } = string.Empty;
        public DateTime DateCreated { get; internal set; }
    }
}
