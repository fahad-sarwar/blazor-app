using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class OrderService : ServiceBase
    {
        private readonly ILogger<OrderService> _logger;

        public OrderService(IHttpClientFactory httpClientFactory, ILogger<OrderService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<PagedOrderResultViewModel?> GetOrders(int page, int pageSize)
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<PagedOrderResultViewModel>($"api/orders?page={page}&pageSize={pageSize}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting a list of orders.");
                return null;
            }
        }

        public async Task<PagedOrderResultViewModel?> GetOrderByOrderNumber(string orderNumber)
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<PagedOrderResultViewModel>($"api/orders?orderNumber={orderNumber}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting an order with '{OrderNumber}'.", orderNumber);
                return null;
            }
        }

        public async Task<OrderViewModel?> GetOrderById(int orderId)
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<OrderViewModel>($"api/orders/{orderId}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting an order with an id of {OrderId}", orderId);
                return null;
            }
        }

        public async Task<OrderViewModel?> CreateOrder(CheckoutViewModel checkoutViewModel, BasketViewModel? basketViewModel)
        {
            try
            {
                var createOrderRequest = new CreateOrderRequest
                {
                    Customer = new CustomerViewModel
                    {
                        Id = checkoutViewModel.CustomerId,
                        FirstName = checkoutViewModel.FirstName,
                        LastName = checkoutViewModel.LastName,
                        Email = checkoutViewModel.Email,
                        PhoneNumber = checkoutViewModel.PhoneNumber,
                        BillingAddress = new AddressViewModel
                        {
                            AddressLineOne = checkoutViewModel.BillingAddressLineOne,
                            AddressLineTwo = checkoutViewModel.BillingAddressLineTwo,
                            Town = checkoutViewModel.BillingTown,
                            County = checkoutViewModel.BillingCounty,
                            PostCode = checkoutViewModel.BillingPostCode,
                            Country = checkoutViewModel.BillingCountry
                        },
                        ShippingAddress = new AddressViewModel
                        {
                            AddressLineOne = checkoutViewModel.ShippingAddressLineOne,
                            AddressLineTwo = checkoutViewModel.ShippingAddressLineTwo,
                            Town = checkoutViewModel.ShippingTown,
                            County = checkoutViewModel.ShippingCounty,
                            PostCode = checkoutViewModel.ShippingPostCode,
                            Country = checkoutViewModel.ShippingCountry
                        }
                    },
                    BasketId = basketViewModel.Id,
                    Payment = new PaymentDetails
                    {
                        CardNumber = checkoutViewModel.CardNumber,
                        CardName = checkoutViewModel.CardName,
                        Expiry = checkoutViewModel.Expiry,
                        CVV = checkoutViewModel.CVV
                    }
                };

                var response = await GetClientFactory().PostAsJsonAsync("api/orders", createOrderRequest);

                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<OrderViewModel>();
                    return order;
                }

                _logger.LogError("There was an error creating the order.  The API response code was '{StatusCode}'", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating an order.");
                return null;
            }
        }
    }
}
