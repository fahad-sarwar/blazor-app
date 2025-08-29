using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class ReviewService(IHttpClientFactory httpClientFactory)
    {
        public async Task<PagedReviewResultViewModel> GetPagedReviewsAsync(int productId, int page, int pageSize)
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.GetFromJsonAsync<PagedReviewResultViewModel>($"api/reviews?productId={productId}&page={page}&pageSize={pageSize}");
            return response ?? new PagedReviewResultViewModel();
        }

        public async Task<bool> CreateReviewAsync(CreateReviewViewModel createReviewViewModel)
        {
            var httpClient = httpClientFactory.CreateClient("Api");

            var response = await httpClient.PostAsJsonAsync("api/reviews", createReviewViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
