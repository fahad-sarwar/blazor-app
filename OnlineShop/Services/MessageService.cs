using Api.Models;

namespace OnlineShopUI.Services
{
    public class MessageService : ServiceBase
    {
        private readonly ILogger<MessageService> _logger;

        public MessageService(IHttpClientFactory httpClientFactory, ILogger<MessageService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<bool> SendMessage(Message message)
        {
            var response = await GetClientFactory().PostAsJsonAsync("api/messages", message);
            return response.IsSuccessStatusCode;
        }
    }
}
