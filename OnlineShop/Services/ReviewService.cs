using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class ReviewService(IHttpClientFactory httpClientFactory, ILogger<ReviewService> logger)
    {
        public async Task<PagedReviewResultViewModel?> GetPagedReviews(int productId, int page, int pageSize)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<PagedReviewResultViewModel>($"api/reviews?productId={productId}&page={page}&pageSize={pageSize}");
                return response ?? null;
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting for product reviews for id {ProductId}", productId);
                return null;
            }
        }

        public async Task<bool> CreateReview(CreateReviewViewModel createReviewViewModel)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");

                var response = await httpClient.PostAsJsonAsync("api/reviews", createReviewViewModel);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating review for product id {ProductId}", createReviewViewModel.ProductId);
                return false;
            }
        }
    }
}
