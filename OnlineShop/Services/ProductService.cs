using System.Text.Json;
using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class ProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IHttpClientFactory httpClientFactory, ILogger<ProductService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<PagedProductResultViewModel?> GetProducts(string searchTerm, string selectedSort, int currentPage, int pageSize)
        {
            var pagedProductResult = await _httpClientFactory.CreateClient("Api").GetFromJsonAsync<PagedProductResultViewModel>(
                $"api/products?searchTerm={searchTerm}&sort={selectedSort}&page={currentPage}&pageSize={pageSize}");

            return pagedProductResult;
        }

        public async Task<PagedProductResultViewModel?> GetProducts(string selectedSort, int currentPage, int pageSize)
        {
            var pagedProductResult = await _httpClientFactory.CreateClient("Api").GetFromJsonAsync<PagedProductResultViewModel>(
                $"api/products?forSale=true&sort={selectedSort}&page={currentPage}&pageSize={pageSize}");

            return pagedProductResult;
        }

        public async Task<PagedProductResultViewModel?> GetProducts(int? categoryId, string selectedSort, int currentPage, int pageSize)
        {
            if (categoryId == null)
            {
                _logger.LogWarning("The category id is required.  Please provide the correct details.");
                return null;
            }

            var pagedProductResult = await _httpClientFactory.CreateClient("Api").GetFromJsonAsync<PagedProductResultViewModel>(
                $"api/products?categoryId={categoryId}&sort={selectedSort}&page={currentPage}&pageSize={pageSize}");

            return pagedProductResult;
        }

        public async Task<ProductViewModel> GetProduct(int productId)
        {
            var response = await _httpClientFactory.CreateClient("Api").GetFromJsonAsync<ProductViewModel>($"api/products/{productId}");

            var ratingResponse = await _httpClientFactory.CreateClient("Api").GetFromJsonAsync<JsonElement>($"api/reviews/stats?productId={productId}");

            if (ratingResponse.TryGetProperty("averageRating", out var ratingValue) && ratingValue.ValueKind != JsonValueKind.Null)
            {
                response.AverageRating = ratingValue.GetDouble();
            }

            return response;
        }
    }
}
