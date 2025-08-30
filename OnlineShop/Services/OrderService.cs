using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class OrderService(IHttpClientFactory httpClientFactory)
    {
        public async Task<PagedOrderResultViewModel> GetOrdersAsync(int page, int pageSize)
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.GetFromJsonAsync<PagedOrderResultViewModel>($"api/orders?page={page}&pageSize={pageSize}");
            return response ?? new PagedOrderResultViewModel();
        }

        public async Task<OrderViewModel> GetOrderAsync(int orderId)
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.GetFromJsonAsync<OrderViewModel>($"api/orders/{orderId}");
            return response ?? new OrderViewModel();
        }
    }
}
