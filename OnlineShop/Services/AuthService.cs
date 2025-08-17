using Microsoft.AspNetCore.Components.Authorization;
using OnlineShopUI.ViewModels;
using System.Text.Json;

namespace OnlineShopUI.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginViewModel loginModel);
        Task<bool> RegisterAsync(RegisterViewModel registerModel);
        Task LogoutAsync();
        Task<UserInfo?> GetCurrentUserAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IHttpClientFactory httpClientFactory,
            AuthenticationStateProvider authenticationStateProvider,
            ILogger<AuthService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _authenticationStateProvider = authenticationStateProvider;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(LoginViewModel loginModel)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Api");
                var result = await httpClient.PostAsJsonAsync("api/auth/login", loginModel);

                if (result.IsSuccessStatusCode)
                {
                    var user = await GetUserFromApiAsync();
                    if (user != null && _authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                    {
                        await apiProvider.SetAuthenticationStateAsync(user);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
            }

            return false;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerModel)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Api");
                var result = await httpClient.PostAsJsonAsync("api/auth/register", registerModel);

                if (result.IsSuccessStatusCode)
                {
                    var user = await GetUserFromApiAsync();
                    if (user != null && _authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                    {
                        await apiProvider.SetAuthenticationStateAsync(user);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Api");
                await httpClient.PostAsync("api/auth/logout", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during API logout");
            }
            finally
            {
                // Always clear local state
                if (_authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                {
                    await apiProvider.SetAuthenticationStateAsync(null);
                }
            }
        }

        public async Task<UserInfo?> GetCurrentUserAsync()
        {
            return await GetUserFromApiAsync();
        }

        private async Task<UserInfo?> GetUserFromApiAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Api");
                var userResponse = await httpClient.GetAsync("api/auth/user");

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserInfo>(userJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user from API");
            }

            return null;
        }
    }
}
