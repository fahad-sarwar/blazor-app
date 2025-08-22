using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace OnlineShopUI.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<ApiAuthenticationStateProvider> _logger;

        public ApiAuthenticationStateProvider(ProtectedLocalStorage localStorage,
            ILogger<ApiAuthenticationStateProvider> logger)
        {
            _localStorage = localStorage;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await _localStorage.GetAsync<UserInfo>("user");

                if (result.Success && result.Value != null && !string.IsNullOrEmpty(result.Value.Email))
                {
                    var user = result.Value;
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.FirstName ?? string.Empty),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                        new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                        new Claim("UserId", user.UserId ?? string.Empty),
                        new Claim("CustomerId", user.CustomerId.ToString() ?? string.Empty)
                    };

                    var identity = new ClaimsIdentity(claims, "apiauth");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting authentication state");
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task SetAuthenticationStateAsync(UserInfo? user)
        {
            if (user != null)
            {
                await _localStorage.SetAsync("user", user);
            }
            else
            {
                await _localStorage.DeleteAsync("user");
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public class UserInfo
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserId { get; set; }
        public int? CustomerId { get; set; }
    }
}
