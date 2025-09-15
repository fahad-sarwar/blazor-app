using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class BasketService(IHttpClientFactory httpClientFactory, AnonymousUserService anonymousUserService, BasketCountService basketCountService, ILogger<BasketService> logger)
        : ServiceBase(httpClientFactory)
    {
        public async Task<BasketViewModel?> GetBasket()
        {
            try
            {
                var anonymousUserId = await anonymousUserService.GetOrCreateAnonymousId();

                var basketViewModel = await GetClientFactory().GetFromJsonAsync<BasketViewModel>($"api/Baskets?anonymousUserId={anonymousUserId}");

                return basketViewModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading basket");
                return null;
            }
        }

        public async Task<bool> AddToBasket(int productId, int quantity)
        {
            try
            {
                var anonymousUserId = await anonymousUserService.GetOrCreateAnonymousId();

                var existingBasket = await GetBasket();

                var existingItem = existingBasket.Items.FirstOrDefault(item => item.Product.Id == productId);

                if (existingItem != null)
                {
                    return await UpdateBasketQuantity(existingItem.Id, existingItem.Quantity + quantity);
                }

                var basketItem = new
                {
                    AnonymousId = anonymousUserId,
                    ProductId = productId,
                    Quantity = quantity
                };

                var response = await GetClientFactory().PostAsJsonAsync("api/BasketItems", basketItem);

                if (response.IsSuccessStatusCode)
                {
                    basketCountService.Increment(1);
                    return true;
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error adding product {ProductId} to basket", productId);
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

                var response = await GetClientFactory().PutAsJsonAsync($"api/BasketItems/{basketItemId}", updatedItem);

                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error updating basket item {BasketItemId} quantity", basketItemId);
            }

            return false;
        }

        public async Task<bool> RemoveItemFromBasket(int basketItemId)
        {
            try
            {
                var response = await GetClientFactory().DeleteAsync($"api/BasketItems/{basketItemId}");

                if (response.IsSuccessStatusCode)
                {
                    basketCountService.Decrement(1);
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing basket item {BasketItemId} quantity", basketItemId);
            }

            return false;
        }
    }
}
