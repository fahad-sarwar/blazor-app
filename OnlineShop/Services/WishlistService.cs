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

        public async Task<PagedProductResultViewModel?> GetWishlist(int page, int pageSize)
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<PagedProductResultViewModel>($"api/wishlist?page={page}&pageSize={pageSize}");
                return response;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting products on the customers wishlist.");
                return null;
            }
        }

        public async Task<bool> IsOnWishlist(int productId)
        {
            try
            {
                var response = await GetClientFactory().GetAsync($"api/wishlist/{productId}/exists");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error checking if the product with an id of {ProductId} is on the customers wishlist.", productId);
                return false;
            }
        }

        public async Task<bool> AddToWishlist(int productId)
        {
            try
            {
                var request = new
                {
                    ProductId = productId
                };

                var response = await GetClientFactory().PostAsJsonAsync("api/wishlist", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error adding a product with an id of {ProductId} to the customers wishlist.", productId);
                return false;
            }
        }

        public async Task<bool> RemoveFromWishlist(int productId)
        {
            try
            {
                var response = await GetClientFactory().DeleteAsync($"api/wishlist/{productId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error removing product a product with an id of {ProductId} from the customers wishlist.", productId);
                return false;
            }
        }
    }
}
