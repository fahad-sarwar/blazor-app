using Microsoft.JSInterop;

namespace OnlineShopUI.Services
{
    public class AnonymousUserService
    {
        private readonly IJSRuntime _js;
        private const string AnonymousUserIdKey = "anonymous_user_id";

        public AnonymousUserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string> GetOrCreateAnonymousIdAsync()
        {
            var AnonymousUserId = await _js.InvokeAsync<string>("localStorage.getItem", AnonymousUserIdKey);
            if (string.IsNullOrEmpty(AnonymousUserId))
            {
                AnonymousUserId = Guid.NewGuid().ToString();
                await _js.InvokeVoidAsync("localStorage.setItem", AnonymousUserIdKey, AnonymousUserId);
            }
            return AnonymousUserId;
        }
    }
}