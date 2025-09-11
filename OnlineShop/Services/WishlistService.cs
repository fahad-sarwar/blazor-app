using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class WishlistService(IHttpClientFactory httpClientFactory, ILogger<CategoryService> logger)
    {
        public async Task<PagedProductResultViewModel?> GetWishlistAsync(int page, int pageSize)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<PagedProductResultViewModel>($"api/wishlist?page={page}&pageSize={pageSize}");
                return response;
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting products on customers wishlist");
                return null;
            }
        }

        public async Task<bool> IsOnWishlistAsync(int productId)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetAsync($"api/wishlist/{productId}/exists");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking if product {ProductId} is on wishlist", productId);
                return false;
            }
        }

        public async Task<bool> AddToWishlistAsync(int productId)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");

                var request = new
                {
                    ProductId = productId
                };

                var response = await httpClient.PostAsJsonAsync("api/wishlist", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding product {ProductId} to wishlist", productId);
                return false;
            }
        }

        public async Task<bool> RemoveFromWishlistAsync(int productId)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.DeleteAsync($"api/wishlist/{productId}");
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
