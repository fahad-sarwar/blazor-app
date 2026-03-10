using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class BasketService : ServiceBase
    {
        private readonly AnonymousUserService _anonymousUserService;
        private readonly BasketCountService _basketCountService;

        public BasketService(IHttpClientFactory httpClientFactory, AnonymousUserService anonymousUserService, BasketCountService basketCountService) : base(httpClientFactory)
        {
            _anonymousUserService = anonymousUserService;
            _basketCountService = basketCountService;
        }

        public async Task<BasketViewModel?> GetBasket()
        {
            try
            {
                var anonymousUserId = await _anonymousUserService.GetOrCreateAnonymousId();

                var basketViewModel = await GetClientFactory().GetFromJsonAsync<BasketViewModel>($"api/Baskets?anonymousUserId={anonymousUserId}");

                return basketViewModel;
            }
            catch (Exception ex)
            {
                // TODO should log error 
                return null;
            }
        }

        public async Task<bool> AddToBasket(int productId, int quantity)
        {
            var anonymousUserId = await _anonymousUserService.GetOrCreateAnonymousId();

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
                _basketCountService.Increment(1);
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateBasketQuantity(int basketItemId, int newQuantity)
        {
            var updatedItem = new
            {
                Quantity = newQuantity
            };

            var response = await GetClientFactory().PutAsJsonAsync($"api/BasketItems/{basketItemId}", updatedItem);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveItemFromBasket(int basketItemId)
        {
            var response = await GetClientFactory().DeleteAsync($"api/BasketItems/{basketItemId}");

            if (response.IsSuccessStatusCode)
            {
                _basketCountService.Decrement(1);
                return true;
            }

            return false;
        }
    }
}
