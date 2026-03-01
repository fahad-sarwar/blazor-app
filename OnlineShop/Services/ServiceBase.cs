namespace OnlineShopUI.Services
{
    public class ServiceBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceBase(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient GetClientFactory()
        {
            return _httpClientFactory.CreateClient("Api");
        }
    }
}
