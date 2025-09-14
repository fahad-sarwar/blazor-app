namespace OnlineShopUI.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public double TotalPrice { get; set; }
        public double VATRate { get; set; }
        public string Status { get; set; }
        public string DeliveryMethod { get; set; } = string.Empty;
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomerViewModel Customer { get; set; }
        public AddressViewModel ShippingAddress { get; set; } = new();
        public AddressViewModel BillingAddress { get; set; } = new();
        public PaymentViewModel Payment { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public List<OrderTrackingUpdateViewModel> TrackingUpdates { get; set; } = new List<OrderTrackingUpdateViewModel>(); 
        
        public double Vat()
        {
            return TotalPrice * VATRate;
        }

        public string FormattedVat()
        {
            return Vat().ToString("C");
        }

        public string FormattedTotalPrice()
        {
            return TotalPrice.ToString("C");
        }

        public string GetFormattedCreatedAtDate()
        {
            return CreatedAt.ToLocalTime().ToString("dd MMM yyyy HH:mm");
        }

        public string? GetFormattedEstimatedDeliveryDate()
        {
            return EstimatedDelivery?.ToLocalTime().ToString("dd MMM yyyy");
        }
    }
}
