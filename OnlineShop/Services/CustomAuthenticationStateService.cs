using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CustomAuthenticationStateService : ServiceBase
    {
        private UserInfoViewModel? _userInfoViewModel;

        public CustomAuthenticationStateService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public event Func<Task>? OnUserInfoChanged;

        public UserInfoViewModel? GetUserInfoDetails()
        {
            return _userInfoViewModel;
        }

        public bool IsUserAuthenticated()
        {
            if (_userInfoViewModel == null)
            {
                return false;
            }

            var response = GetClientFactory().GetAsync("api/auth/user")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            ClearUserInfoDetails()
                .GetAwaiter()
                .GetResult();

            return false;
        }

        public async Task SetUserInfoDetails(UserInfoViewModel? userInfoViewModel)
        {
            _userInfoViewModel = userInfoViewModel;
            await NotifyStateChanged();
        }

        public async Task ClearUserInfoDetails()
        {
            _userInfoViewModel = null;
            await NotifyStateChanged();
        }

        private async Task NotifyStateChanged()
        {
            if (OnUserInfoChanged != null)
            {
                await OnUserInfoChanged.Invoke();
            }
        }
    }
}
