using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory, ILogger<ProductService> logger)
    {
        public async Task<PagedProductResultViewModel?> GetProducts(string searchTerm, string selectedSort, int currentPage, int pageSize)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var pagedProductResult = await httpClient.GetFromJsonAsync<PagedProductResultViewModel>(
                    $"api/products?searchTerm={searchTerm}&sort={selectedSort}&page={currentPage}&pageSize={pageSize}");

                return pagedProductResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error searching for products using search term '{SearchTerm}'", searchTerm);
                return null;
            }
        }

        public async Task<PagedProductResultViewModel?> GetProducts(string selectedSort, int currentPage, int pageSize)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var pagedProductResult = await httpClient.GetFromJsonAsync<PagedProductResultViewModel>(
                    $"api/products?forSale=true&sort={selectedSort}&page={currentPage}&pageSize={pageSize}");

                return pagedProductResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting for sale products");
                return null;
            }
        }

        public async Task<PagedProductResultViewModel?> GetProducts(int? categoryId, string selectedSort, int currentPage, int pageSize)
        {
            try
            {
                if (categoryId == null)
                {
                    logger.LogWarning("CategoryId is required.");
                    return null;
                }

                var httpClient = httpClientFactory.CreateClient("Api");
                var pagedProductResult = await httpClient.GetFromJsonAsync<PagedProductResultViewModel>(
                    $"api/products?categoryId={categoryId}&sort={selectedSort}&page={currentPage}&pageSize={pageSize}");

                return pagedProductResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting products in category {CategoryId}", categoryId);
                return null;
            }
        }

        public async Task<ProductViewModel> GetProduct(int productId)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient("Api");
                var response = await httpClient.GetFromJsonAsync<ProductViewModel>($"api/products/{productId}");

                var ratingResponse = await httpClient.GetFromJsonAsync<ProductReviewStatsViewModel>($"api/reviews/stats?productId={productId}");

                response.AverageRating = ratingResponse?.AverageRating ?? 0;

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting product details for id {ProductId}", productId);
                return null;
            }
        }
    }
}
