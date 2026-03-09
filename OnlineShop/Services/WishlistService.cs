using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class WishlistService : ServiceBase
    {
        private readonly ILogger<WishlistService> _logger;

        public WishlistService(IHttpClientFactory httpClientFactory, ILogger<WishlistService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<List<ProductViewModel>?> GetWishlist()
        {
            var response = await GetClientFactory().GetFromJsonAsync<List<ProductViewModel>>($"api/wishlist");
            return response;
        }

        public async Task<bool> IsOnWishlist(int productId)
        {
            var response = await GetClientFactory().GetAsync($"api/wishlist/{productId}/exists");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddToWishlist(int productId)
        {
            var request = new
            {
                ProductId = productId
            };

            var response = await GetClientFactory().PostAsJsonAsync("api/wishlist", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFromWishlist(int productId)
        {
            var response = await GetClientFactory().DeleteAsync($"api/wishlist/{productId}");
            return response.IsSuccessStatusCode;
        }
    }
}
