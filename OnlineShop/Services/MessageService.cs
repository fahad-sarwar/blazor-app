using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class MessageService : ServiceBase
    {
        private readonly ILogger<MessageService> _logger;

        public MessageService(IHttpClientFactory httpClientFactory, ILogger<MessageService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<bool> SendMessage(SendMessageViewModel message)
        {
            try
            {
                var response = await GetClientFactory().PostAsJsonAsync("api/messages", message);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error sending the customers message.");
                return false;
            }
        }
    }
}
