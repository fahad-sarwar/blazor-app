namespace OnlineShopUI.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
        public string DeliveryMethod { get; set; } = string.Empty;
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomerViewModel Customer { get; set; }
        public PaymentViewModel Payment { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public List<OrderTrackingUpdateViewModel> TrackingUpdates { get; set; } = new List<OrderTrackingUpdateViewModel>();
    }
}
