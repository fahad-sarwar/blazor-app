using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class MessageService(IHttpClientFactory httpClientFactory, ILogger<MessageService> logger)
    {
        public async Task<bool> SendMessage(SendMessageViewModel message)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");

                var response = await httpClient.PostAsJsonAsync("api/messages", message);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending message");
                return false;
            }
        }
    }
}
