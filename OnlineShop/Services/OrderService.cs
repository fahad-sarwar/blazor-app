using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class OrderService(IHttpClientFactory httpClientFactory, ILogger<OrderService> logger)
    {
        public async Task<PagedOrderResultViewModel?> GetOrdersAsync(int page, int pageSize)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<PagedOrderResultViewModel>($"api/orders?page={page}&pageSize={pageSize}");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting orders");
                return null;
            }
        }

        public async Task<PagedOrderResultViewModel?> GetOrderByOrderNumberAsync(string orderNumber)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<PagedOrderResultViewModel>($"api/orders?orderNumber={orderNumber}");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting order by {OrderNumber}", orderNumber);
                return null;
            }
        }

        public async Task<OrderViewModel?> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<OrderViewModel>($"api/orders/{orderId}");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting order by {OrderId}", orderId);
                return null;
            }
        }
    }
}
