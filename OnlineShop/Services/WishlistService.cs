using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class WishlistService(IHttpClientFactory httpClientFactory, ILogger<WishlistService> logger) : BaseService(httpClientFactory)
    {
        public async Task<PagedProductResultViewModel?> GetWishlist(int page, int pageSize)
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<PagedProductResultViewModel>($"api/wishlist?page={page}&pageSize={pageSize}");
                return response;
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting products on customers wishlist");
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
                logger.LogError(ex, "Error checking if product {ProductId} is on wishlist", productId);
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
                logger.LogError(ex, "Error adding product {ProductId} to wishlist", productId);
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
                logger.LogError(ex, "Error removing product {ProductId} from wishlist", productId);
                return false;
            }
        }
    }
}
