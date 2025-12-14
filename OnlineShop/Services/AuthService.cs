using Microsoft.AspNetCore.Components.Authorization;
using OnlineShopUI.ViewModels;

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
                    if (authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                    {
                        apiProvider.NotifyAuthenticationStateChangedAsync();
                    }
                    return true;
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
                    if (authenticationStateProvider is ApiAuthenticationStateProvider apiProvider)
                    {
                        apiProvider.NotifyAuthenticationStateChangedAsync();
                    }
                    return true;
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
                    apiProvider.NotifyAuthenticationStateChangedAsync();
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
                    apiProvider.NotifyAuthenticationStateChangedAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error refreshing the customers authentication state.");
            }

            return false;
        }
    }
}
