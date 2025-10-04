using Microsoft.AspNetCore.Components.Authorization;
using OnlineShopUI.ViewModels;
using System.Text.Json;

namespace OnlineShopUI.Services
{
    public class AuthService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider, ILogger<AuthService> logger)
        : ServiceBase(httpClientFactory)
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
                logger.LogError(ex, "There was an error during the registration process.");
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
                logger.LogError(ex, "There was an error logging in the customer.");
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
                logger.LogError(ex, "There was an error logging out the customer.");
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
                logger.LogError(ex, "There was an error refreshing the customers authentication state.");
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
                logger.LogError(ex, "There was an error getting user details.");
            }

            return null;
        }
    }
}
