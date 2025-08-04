namespace Api.Models
{
    public class CreateOrderRequest
    {
        public CreateCustomerRequest Customer { get; set; }
        public int BasketId { get; set; }
        public CreatePaymentRequest Payment { get; set; }
    }
}
