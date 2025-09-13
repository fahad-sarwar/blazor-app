using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CustomerService(IHttpClientFactory httpClientFactory, ILogger<CustomerService> logger) : BaseService(httpClientFactory)
    {
        public async Task<CustomerViewModel?> GetCustomer()
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<CustomerViewModel>($"api/customers");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting customer");
                return null;
            }
        }

        public async Task<bool> UpdateCustomer(UpdateCustomerViewModel request)
        {
            try
            {
                var response = await GetClientFactory().PutAsJsonAsync($"api/customers/{request.Id}", request);

                if (response.IsSuccessStatusCode)
                {
                    return response.IsSuccessStatusCode;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                logger.LogError("Error updating customer with id {CustomerId}.  Response content is '{ResponseContent}'", request.Id, responseContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating customer with id {CustomerId}", request.Id);
                return false;
            }
        }
    }
}
