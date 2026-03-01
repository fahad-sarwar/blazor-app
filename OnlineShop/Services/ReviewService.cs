using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class ReviewService : ServiceBase
    {
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IHttpClientFactory httpClientFactory, ILogger<ReviewService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<PagedReviewResultViewModel?> GetPagedReviews(int productId, int page, int pageSize)
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<PagedReviewResultViewModel>($"api/reviews?productId={productId}&page={page}&pageSize={pageSize}");
                return response ?? null;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting product reviews for product with an id of {ProductId}.", productId);
                return null;
            }
        }

        public async Task<bool> CreateReview(CreateReviewViewModel createReviewViewModel)
        {
            try
            {
                var response = await GetClientFactory().PostAsJsonAsync("api/reviews", createReviewViewModel);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating a product review for product with an id of {ProductId}.", createReviewViewModel.ProductId);
                return false;
            }
        }
    }
}
