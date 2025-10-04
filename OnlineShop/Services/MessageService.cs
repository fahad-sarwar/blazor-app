using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class MessageService(IHttpClientFactory httpClientFactory, ILogger<MessageService> logger) : ServiceBase(httpClientFactory)
    {
        public async Task<bool> SendMessage(SendMessageViewModel message)
        {
            try
            {
                var response = await GetClientFactory().PostAsJsonAsync("api/messages", message);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error sending the customers message.");
                return false;
            }
        }
    }
}
