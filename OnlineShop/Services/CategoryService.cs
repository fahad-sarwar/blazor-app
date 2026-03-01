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
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<List<CategoryViewModel>>("api/categories");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting a list of categories.");
                return null;
            }
        }

        public async Task<CategoryViewModel?> GetCategory(int? categoryId)
        {
            try
            {
                if(categoryId == null)
                {
                    _logger.LogWarning("The category Id is required.  Please provide the correct details.");
                    return null;
                }
                    
                var response = await GetClientFactory().GetFromJsonAsync<CategoryViewModel>($"api/categories/{categoryId}");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting a category with id {Category}.", categoryId);
                return null;
            }
        }
    }
}
