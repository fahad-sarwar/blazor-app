using Api.Models;

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
        public Customer Customer { get; set; }
        public Address ShippingAddress { get; set; } = new();
        public Address BillingAddress { get; set; } = new();
        public Payment Payment { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<OrderTrackingUpdate> TrackingUpdates { get; set; } = new List<OrderTrackingUpdate>(); 
        
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
