namespace OnlineShopUI.Services
{
    public class BaseService(IHttpClientFactory httpClientFactory)
    {
        public HttpClient GetClientFactory()
        {
            return httpClientFactory.CreateClient("Api");
        }
    }
}
