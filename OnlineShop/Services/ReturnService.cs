using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class ReturnService(IHttpClientFactory httpClientFactory)
    {
        public async Task<bool> CreateReturnAsync(CreateReturnViewModel request)
        {
            var httpClient = httpClientFactory.CreateClient("Api");

            var response = await httpClient.PostAsJsonAsync("api/returns", request);
            return response.IsSuccessStatusCode;
        }
    }
}
