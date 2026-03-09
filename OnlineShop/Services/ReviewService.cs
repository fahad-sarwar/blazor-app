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

        public async Task<List<ReviewViewModel>> GetPagedReviews(int productId)
        {
            var response = await GetClientFactory().GetFromJsonAsync<List<ReviewViewModel>>($"api/reviews?productId={productId}");
            return response;
        }

        public async Task<bool> CreateReview(CreateReviewViewModel createReviewViewModel)
        {
            var response = await GetClientFactory().PostAsJsonAsync("api/reviews", createReviewViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
