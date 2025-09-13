using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace OnlineShopUI.Services
{
    public class ApiAuthenticationStateProvider(
        IHttpClientFactory httpClientFactory,
        ILogger<ApiAuthenticationStateProvider> logger)
        : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetAsync("api/auth/user");

                if (response.IsSuccessStatusCode)
                {
                    var userJson = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<UserInfo>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, user.FirstName ?? string.Empty),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                            new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                            new Claim("UserId", user.UserId ?? string.Empty),
                            new Claim("CustomerId", user.CustomerId.ToString() ?? string.Empty)
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        return new AuthenticationState(new ClaimsPrincipal(identity));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting authentication state");
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task NotifyAuthenticationStateChangedAsync()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public string GetFirstname(AuthenticationState authenticationState)
        {
            var firstNameClaim = authenticationState.User.FindFirst(ClaimTypes.GivenName);

            return !string.IsNullOrEmpty(firstNameClaim?.Value)
                ? firstNameClaim.Value
                : authenticationState.User.Identity?.Name ?? "User";
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
        public string? PhoneNumber { get; set; }
    }
}
