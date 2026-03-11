using Api.Models;

namespace OnlineShopUI.Services
{
    public class CategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IHttpClientFactory httpClientFactory, ILogger<CategoryService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<Category>?> GetCategories()
        {
            var response = await _httpClientFactory.CreateClient("Api").GetFromJsonAsync<List<Category>>("api/categories");

            return response;
        }

        public async Task<Category?> GetCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                _logger.LogWarning("The category Id is required.  Please provide the correct details.");
                return null;
            }

            var response = await _httpClientFactory.CreateClient("Api").GetFromJsonAsync<Category>($"api/categories/{categoryId}");

            return response;
        }
    }
}
