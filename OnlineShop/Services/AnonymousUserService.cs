using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace OnlineShopUI.Services
{
    public class AnonymousUserService
    {
        private readonly ProtectedSessionStorage _protectedSessionStorage;

        public AnonymousUserService(ProtectedSessionStorage protectedSessionStorage)
        {
            _protectedSessionStorage = protectedSessionStorage;
        }

        private const string AnonymousUserIdKey = "anonymous_user_id";

        // get or creates a user if stored in the browser session
        public async Task<string> GetOrCreateAnonymousId()
        {
            var result = await _protectedSessionStorage.GetAsync<string>(AnonymousUserIdKey);

            if (result.Success)
                return result.Value;

            var anonymousUserIdValue = Guid.NewGuid().ToString();

            await _protectedSessionStorage.SetAsync(AnonymousUserIdKey, anonymousUserIdValue);

            return anonymousUserIdValue;
        }
    }
}