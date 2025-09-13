using Microsoft.AspNetCore.Components.Authorization;
using OnlineShopUI.ViewModels;
using System.Text.Json;

namespace OnlineShopUI.Services
{
    public class AuthService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider, ILogger<AuthService> logger)
        : BaseService(httpClientFactory)
    {
        public async Task<bool> LoginAsync(LoginViewModel loginModel)
        {
            try
            {
                var result = await GetClientFactory().PostAsJsonAsync("api/auth/login", loginModel);

                if (result.IsSuccessStatusCode)
                {
                    var user = await GetUserFromApiAsync();
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

        public async Task<bool> RegisterAsync(RegisterViewModel registerModel)
        {
            try
            {
                var result = await GetClientFactory().PostAsJsonAsync("api/auth/register", registerModel);

                if (result.IsSuccessStatusCode)
                {
                    var user = await GetUserFromApiAsync();
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

        public async Task LogoutAsync()
        {
            try
            {
                await GetClientFactory().PostAsync("api/auth/logout", null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during API logout");
            }
            finally
            {
                // Always clear local state
                if (authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                {
                    await apiProvider.NotifyAuthenticationStateChangedAsync();
                }
            }
        }

        public async Task<bool> UpdateProfileAsync(UpdateCustomerViewModel updateModel)
        {
            try
            {
                var response = await GetClientFactory().PutAsJsonAsync($"api/customers/{updateModel.Id}", updateModel);

                if (response.IsSuccessStatusCode)
                {
                    await RefreshAuthenticationStateAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating profile");
            }

            return false;
        }

        public async Task<bool> RefreshAuthenticationStateAsync()
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

        private async Task<UserInfo?> GetUserFromApiAsync()
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
