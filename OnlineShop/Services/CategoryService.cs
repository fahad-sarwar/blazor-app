using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CategoryService(IHttpClientFactory httpClientFactory, ILogger<CategoryService> logger) : ServiceBase(httpClientFactory)
    {
        public async Task<List<CategoryViewModel>?> GetCategories()
        {
            try
            {
                var response = await GetClientFactory().GetFromJsonAsync<List<CategoryViewModel>>("api/categories");

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting categories");
                return null;
            }
        }

        public async Task<CategoryViewModel?> GetCategory(int? categoryId)
        {
            try
            {
                if(categoryId == null)
                {
                    logger.LogWarning("CategoryId is required.");
                    return null;
                }
                    
                var response = await GetClientFactory().GetFromJsonAsync<CategoryViewModel>($"api/categories/{categoryId}");

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting categories");
                return null;
            }
        }
    }
}
