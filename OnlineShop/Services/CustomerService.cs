using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CustomerService(IHttpClientFactory httpClientFactory, ILogger<CustomerService> logger) : ServiceBase(httpClientFactory)
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
                logger.LogError(ex, "There was an error getting the customers details.");
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
                logger.LogError("There was an error updating the customers details with id {Customer}.  The API responded with '{ResponseContent}'", request.Id, responseContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error updating the customers details with id {Customer}"., request.Id);
                return false;
            }
        }
    }
}
