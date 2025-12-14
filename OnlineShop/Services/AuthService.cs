using Microsoft.AspNetCore.Components.Authorization;
using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class AuthService
        (
            IHttpClientFactory httpClientFactory, 
            AuthenticationStateProvider authenticationStateProvider,
            CustomAuthenticationStateService customAuthenticationStateService,
            ILogger<AuthService> logger
        )
        : ServiceBase(httpClientFactory)
    {
        public async Task<bool> Register(RegisterViewModel registerModel)
        {
            try
            {
                var result = await GetClientFactory().PostAsJsonAsync("api/auth/register", registerModel);

                if (result.IsSuccessStatusCode)
                {
                    var user = await GetClientFactory().GetFromJsonAsync<UserInfoViewModel>("api/auth/user");
                    await customAuthenticationStateService.SetUserInfoDetails(user);

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
                    var user = await GetClientFactory().GetFromJsonAsync<UserInfoViewModel>("api/auth/user");
                    await customAuthenticationStateService.SetUserInfoDetails(user);

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
                await customAuthenticationStateService.ClearUserInfoDetails();
                result = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error logging out the customer.");
            }

            return result;
        }

        public async Task<bool> RefreshAuthenticationState()
        {
            try
            {
                await GetClientFactory().PostAsync("api/auth/refresh-claims", null);

                var user = await GetClientFactory().GetFromJsonAsync<UserInfoViewModel>("api/auth/user");
                await customAuthenticationStateService.SetUserInfoDetails(user);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error refreshing the customers authentication state.");
            }

            return false;
        }
    }
}
