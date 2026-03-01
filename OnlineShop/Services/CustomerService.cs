using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CustomerService : ServiceBase
    {
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IHttpClientFactory httpClientFactory, ILogger<CustomerService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<CustomerViewModel?> GetCustomer()
        {
            var response = await GetClientFactory().GetFromJsonAsync<CustomerViewModel>($"api/customers");
            return response;
        }

        public async Task<bool> UpdateCustomer(UpdateCustomerViewModel request)
        {
            var response = await GetClientFactory().PutAsJsonAsync($"api/customers/{request.Id}", request);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("There was an error updating the customers details with id {Customer}.  The API responded with '{ResponseContent}'", request.Id, responseContent);

            return response.IsSuccessStatusCode;
        }
    }
}
