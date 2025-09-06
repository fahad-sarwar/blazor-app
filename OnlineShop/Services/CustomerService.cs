using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CustomerService(IHttpClientFactory httpClientFactory)
    {
        public async Task<CustomerViewModel> GetCustomerAsync()
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.GetFromJsonAsync<CustomerViewModel>($"api/customers");
            return response;
        }

        public async Task<bool> UpdateCustomerAsync(UpdateCustomerViewModel request)
        {
            var httpClient = httpClientFactory.CreateClient("Api");

            var response = await httpClient.PutAsJsonAsync($"api/customers/{request.Id}", request);
            return response.IsSuccessStatusCode;
        }
    }
}
