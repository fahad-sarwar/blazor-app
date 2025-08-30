using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class MessageService(IHttpClientFactory httpClientFactory)
    {
        public async Task<bool> SendMessageAsync(SendMessageViewModel message)
        {
            var httpClient = httpClientFactory.CreateClient("Api");

            var response = await httpClient.PostAsJsonAsync("api/messages", message);
            return response.IsSuccessStatusCode;
        }
    }
}
