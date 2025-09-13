using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CategoryService(IHttpClientFactory httpClientFactory, ILogger<CategoryService> logger)
    {
        public async Task<List<CategoryViewModel>?> GetCategories()
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<List<CategoryViewModel>>("api/categories");

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
                    logger.LogWarning("CategoryId is null");
                    return null;
                }
                    
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<CategoryViewModel>($"api/categories/{categoryId}");

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
