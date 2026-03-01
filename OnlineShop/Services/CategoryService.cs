using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CategoryService : ServiceBase
    {
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IHttpClientFactory httpClientFactory, ILogger<CategoryService> logger) : base(httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<List<CategoryViewModel>?> GetCategories()
        {
            var response = await GetClientFactory().GetFromJsonAsync<List<CategoryViewModel>>("api/categories");

            return response;
        }

        public async Task<CategoryViewModel?> GetCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                _logger.LogWarning("The category Id is required.  Please provide the correct details.");
                return null;
            }

            var response = await GetClientFactory().GetFromJsonAsync<CategoryViewModel>($"api/categories/{categoryId}");

            return response;
        }
    }
}
