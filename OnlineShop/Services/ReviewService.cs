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
            var response = await GetClientFactory().GetFromJsonAsync<PagedReviewResultViewModel>($"api/reviews?productId={productId}&page={page}&pageSize={pageSize}");
            return response;
        }

        public async Task<bool> CreateReview(CreateReviewViewModel createReviewViewModel)
        {
            var response = await GetClientFactory().PostAsJsonAsync("api/reviews", createReviewViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
