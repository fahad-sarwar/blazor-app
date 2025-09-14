namespace OnlineShopUI.ViewModels
{
    public class OrderTrackingUpdateViewModel
    {
        public string UpdatedBy { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string GetFormattedCreatedAtDate()
        {
            return CreatedAt.ToLocalTime().ToString("dd MMM yyyy HH:mm");
        }
    }
}
