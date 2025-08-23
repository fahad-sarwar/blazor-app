using OnlineShopUI.ViewModels;
using System.Net.Http;

namespace OnlineShopUI.Services
{
    public class WishlistService(IHttpClientFactory httpClientFactory)
    {
        public async Task<List<ProductViewModel>> GetWishlistAsync(int page, int pageSize)
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.GetFromJsonAsync<List<ProductViewModel>>($"api/wishlist?page={page}&pageSize={pageSize}");
            return response ?? new List<ProductViewModel>();
        }

        public async Task<bool> IsOnWishlistAsync(int productId)
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.GetAsync($"api/wishlist/{productId}/exists");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddToWishlistAsync(int productId)
        {
            var httpClient = httpClientFactory.CreateClient("Api");

            var request = new
            {
                ProductId = productId
            };

            var response = await httpClient.PostAsJsonAsync("api/wishlist", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFromWishlistAsync(int productId)
        {
            var httpClient = httpClientFactory.CreateClient("Api");
            var response = await httpClient.DeleteAsync($"api/wishlist/{productId}");
            return response.IsSuccessStatusCode;
        }
    }
}
