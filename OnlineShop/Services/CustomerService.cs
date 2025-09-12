using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CustomerService(IHttpClientFactory httpClientFactory, ILogger<CustomerService> logger)
    {
        public async Task<CustomerViewModel?> GetCustomerAsync()
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<CustomerViewModel>($"api/customers");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting customer");
                return null;
            }
        }

        public async Task<bool> UpdateCustomerAsync(UpdateCustomerViewModel request)
        {
            var httpClient = httpClientFactory.CreateClient("Api");

            var response = await httpClient.PutAsJsonAsync($"api/customers/{request.Id}", request);
            return response.IsSuccessStatusCode;
        }
    }
}
