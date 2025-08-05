using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class BasketService(IHttpClientFactory httpClientFactory, AnonymousUserService anonymousUserService, BasketCountService basketCountService)
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Api");

        public async Task<BasketViewModel> GetBasket()
        {
            try
            {
                var anonymousUserId = await anonymousUserService.GetOrCreateAnonymousIdAsync();

                var basketViewModel = await _httpClient.GetFromJsonAsync<BasketViewModel>($"api/Baskets?anonymousUserId={anonymousUserId}");

                return basketViewModel ?? new BasketViewModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading basket: {ex.Message}");
                throw new Exception("Failed to load basket", ex);
            }
        }

        public async Task<bool> AddToBasketAsync(int productId, int quantity)
        {
            try
            {
                var anonymousUserId = await anonymousUserService.GetOrCreateAnonymousIdAsync();

                // Check if product exists in the basket
                var existingBasket = await GetBasket();

                var existingItem = existingBasket.Items.FirstOrDefault(item => item.Product.Id == productId);

                if (existingItem != null)
                {
                    // Update quantity if item already exists
                    return await UpdateBasketQuantity(existingItem.Id, existingItem.Quantity + quantity);
                }

                var basketItem = new
                {
                    AnonymousId = anonymousUserId,
                    ProductId = productId,
                    Quantity = quantity
                };

                var response = await _httpClient.PostAsJsonAsync("api/BasketItems", basketItem);

                if (response.IsSuccessStatusCode)
                {
                    basketCountService.Increment(1);
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error adding to basket: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> UpdateBasketQuantity(int basketItemId, int newQuantity)
        {
            try
            {
                var updatedItem = new
                {
                    Quantity = newQuantity
                };

                var response = await _httpClient.PutAsJsonAsync($"api/BasketItems/{basketItemId}", updatedItem);

                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error updating basket quantity: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> RemoveItemFromBasket(int basketItemId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/BasketItems/{basketItemId}");

                if (response.IsSuccessStatusCode)
                {
                    basketCountService.Decrement(1);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item from basket: {ex.Message}");
            }

            return false;
        }
    }
}
