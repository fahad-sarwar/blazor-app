using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory)
    {

        public async Task<ProductViewModel> GetProductAsync(int productId)
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.GetFromJsonAsync<ProductViewModel>($"api/products/{productId}");

            var ratingResponse = await httpClient.GetFromJsonAsync<ProductReviewStatsViewModel>($"api/reviews/stats?productId={productId}");

            response.AverageRating = ratingResponse?.AverageRating ?? 0;

            return response;
        }
    }
}
