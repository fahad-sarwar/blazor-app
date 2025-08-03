namespace OnlineShopUI.ViewModels
{
    public class CreateOrderRequest
    {
        public CustomerViewModel Customer { get; set; }
        public int BasketId { get; set; }
        public PaymentDetails Payment { get; set; }
    }
}
