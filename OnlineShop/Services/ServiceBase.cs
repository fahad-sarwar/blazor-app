namespace OnlineShopUI.Services
{
    public class ServiceBase(IHttpClientFactory httpClientFactory)
    {
        public HttpClient GetClientFactory()
        {
            return httpClientFactory.CreateClient("Api");
        }
    }
}
