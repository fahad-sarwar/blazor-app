using Api.Models;
using Api.Models.DTOs;

namespace OnlineShopUI.Services
{
    public class ReviewService : ServiceBase
    {
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IHttpClientFactory httpClientFactory, ILogger<ReviewService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<List<Review>> GetPagedReviews(int productId)
        {
            var response = await GetClientFactory().GetFromJsonAsync<List<Review>>($"api/reviews?productId={productId}");
            return response;
        }

        public async Task<bool> CreateReview(CreateReviewDTO createReviewDTO)
        {
            var response = await GetClientFactory().PostAsJsonAsync("api/reviews", createReviewDTO);
            return response.IsSuccessStatusCode;
        }
    }
}
