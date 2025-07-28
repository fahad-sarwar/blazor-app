using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace OnlineShopUI.Services
{
    public class AnonymousUserService(ProtectedSessionStorage protectedSessionStorage)
    {
        private const string AnonymousUserIdKey = "anonymous_user_id";

        public async Task<string> GetOrCreateAnonymousIdAsync()
        {
            var result = await protectedSessionStorage.GetAsync<string>(AnonymousUserIdKey);

            if (result.Success)
                return result.Value;

            var anonymousUserIdValue = Guid.NewGuid().ToString();
            await protectedSessionStorage.SetAsync(AnonymousUserIdKey, anonymousUserIdValue);
            return anonymousUserIdValue;
        }
    }
}