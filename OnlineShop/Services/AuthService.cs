using Microsoft.AspNetCore.Components.Authorization;
using OnlineShopUI.ViewModels;
using System.Text.Json;

namespace OnlineShopUI.Services
{
    public class AuthService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider, ILogger<AuthService> logger)
        : BaseService(httpClientFactory)
    {
        public async Task<bool> Register(RegisterViewModel registerModel)
        {
            try
            {
                var result = await GetClientFactory().PostAsJsonAsync("api/auth/register", registerModel);

                if (result.IsSuccessStatusCode)
                {
                    var user = await GetUserFromApi();
                    if (user != null && authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                    {
                        await apiProvider.NotifyAuthenticationStateChangedAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during registration");
            }

            return false;
        }

        public async Task<bool> Login(LoginViewModel loginModel)
        {
            try
            {
                var result = await GetClientFactory().PostAsJsonAsync("api/auth/login", loginModel);

                if (result.IsSuccessStatusCode)
                {
                    var user = await GetUserFromApi();
                    if (user != null && authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                    {
                        await apiProvider.NotifyAuthenticationStateChangedAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during login");
            }

            return false;
        }

        public async Task<bool> Logout()
        {
            var result = false;

            try
            {
                await GetClientFactory().PostAsync("api/auth/logout", null);
                result = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during API logout");
            }
            finally
            {
                if (authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                {
                    await apiProvider.NotifyAuthenticationStateChangedAsync();
                }
            }

            return result;
        }

        public async Task<bool> RefreshAuthenticationState()
        {
            try
            {
                await GetClientFactory().PostAsync("api/auth/refresh-claims", null);

                if (authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                {
                    await apiProvider.NotifyAuthenticationStateChangedAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error refreshing authentication state");
            }

            return false;
        }

        private async Task<UserInfo?> GetUserFromApi()
        {
            try
            {
                var userResponse = await GetClientFactory().GetAsync("api/auth/user");

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserInfo>(userJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting user from API");
            }

            return null;
        }
    }
}
