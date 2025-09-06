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
        Task<bool> UpdateProfileAsync(UpdateCustomerViewModel updateModel);
        Task RefreshAuthenticationStateAsync();
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
                        await apiProvider.NotifyAuthenticationStateChangedAsync();
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
                        await apiProvider.NotifyAuthenticationStateChangedAsync();
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
                    await apiProvider.NotifyAuthenticationStateChangedAsync();
                }
            }
        }

        public async Task<UserInfo?> GetCurrentUserAsync()
        {
            return await GetUserFromApiAsync();
        }

        public async Task<bool> UpdateProfileAsync(UpdateCustomerViewModel updateModel)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Api");
                var response = await httpClient.PutAsJsonAsync($"api/customers/{updateModel.Id}", updateModel);

                if (response.IsSuccessStatusCode)
                {
                    // Refresh authentication state to update claims
                    await RefreshAuthenticationStateAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
            }

            return false;
        }

        public async Task RefreshAuthenticationStateAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Api");
                await httpClient.PostAsync("api/auth/refresh-claims", null);

                if (_authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                {
                    await apiProvider.NotifyAuthenticationStateChangedAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing authentication state");
            }
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
