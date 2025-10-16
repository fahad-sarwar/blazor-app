using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class ProductRepository : RepositoryBase
    {
        public async Task<(List<Product> Products, int TotalCount)> GetProducts(int? categoryId, bool? forSale, string? searchTerm, string sort, int page, int pageSize)
        {
            var productCountQuery = 
                "SELECT COUNT(*) FROM Product p " +
                "INNER JOIN Category c ON p.CategoryId = c.Id ";

            var productSearchQuery =
                "SELECT p.Id, p.Name, p.Description, p.Price, p.ImageURL, p.Stock, p.ForSale, p.SalePrice, p.CreatedAt, c.Id, c.Name, c.Description, c.CreatedAt " +
                "FROM Product p " +
                " INNER JOIN Category c ON p.CategoryId = c.Id ";

            var parameters = new DynamicParameters();

            if(IncludeCategoryFilter(categoryId) || IncludeForSaleFilter(forSale) || IncludeSearchTermFilter(searchTerm))
            {
                var whereFilters = new List<string>();

                if (IncludeCategoryFilter(categoryId))
                {
                    whereFilters.Add("p.CategoryId = @categoryId");
                    parameters.Add("@categoryId", categoryId.Value);
                }

                if (IncludeForSaleFilter(forSale))
                {
                    whereFilters.Add("p.ForSale = @forSale");
                    parameters.Add("@forSale", forSale.Value);
                }

                if (IncludeSearchTermFilter(searchTerm))
                {
                    whereFilters.Add("(LOWER(p.Name) LIKE @searchTerm OR LOWER(p.Description) LIKE @searchTerm)");
                    parameters.Add("@searchTerm", $"%{searchTerm.ToLower()}%");
                }

                var whereClause = whereFilters.Count > 0
                    ? "WHERE " + string.Join(" AND ", whereFilters)
                    : "";

                productCountQuery += whereClause;
                productSearchQuery += whereClause;
            }

            string orderBy;
            switch (sort)
            {
                case "name-desc":
                    orderBy = "ORDER BY p.Name DESC";
                    break;
                case "price-asc":
                    orderBy = "ORDER BY p.Price ASC";
                    break;
                case "price-desc":
                    orderBy = "ORDER BY p.Price DESC";
                    break;
                default:
                    orderBy = "ORDER BY p.Name ASC";
                    break;
            }

            productSearchQuery += $" {orderBy}";
            productSearchQuery += " LIMIT @pageSize OFFSET @offset";

            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", (page - 1) * pageSize);

            await using var conn = new SqliteConnection(ConnectionString);

            var totalCount = await conn.QuerySingleAsync<int>(productCountQuery, parameters);

            var productData = await conn.QueryAsync<Product, Category, Product>(
                productSearchQuery,
                (product, category) =>
                {
                    product.Category = category;
                    return product;
                },
                parameters,
                splitOn: "Id"
            );

            return (productData.ToList(), totalCount);
        }

        public async Task<Product?> GetProduct(int productId)
        {
            var query =
                "SELECT p.Id, p.Name, p.Description, p.Price, p.ImageURL, p.Stock, p.ForSale, p.SalePrice, p.CreatedAt, " +
                "c.Id, c.Name, c.Description, c.CreatedAt " +
                "FROM Product p " +
                "INNER JOIN Category c ON p.CategoryId = c.Id " +
                "WHERE p.Id = @productId";

            var attributesQuery =
                "SELECT Id, Name, Value " +
                "FROM ProductAttribute " +
                "WHERE ProductId = @productId";

            await using var conn = new SqliteConnection(ConnectionString);

            var product = await conn.QueryAsync<Product, Category, Product>(
                query,
                (product, category) =>
                {
                    product.Category = category;
                    return product;
                },
                new { productId },
                splitOn: "Id"
            );

            var result = product.FirstOrDefault();

            if (result != null)
            {
                var attributes = await conn.QueryAsync<ProductAttribute>(attributesQuery, new { productId });
                result.Attributes = attributes.ToList();
            }

            return result;
        }

        private static bool IncludeSearchTermFilter(string? searchTerm)
        {
            return !string.IsNullOrWhiteSpace(searchTerm);
        }

        private static bool IncludeForSaleFilter(bool? forSale)
        {
            return forSale.HasValue;
        }

        private static bool IncludeCategoryFilter(int? categoryId)
        {
            return categoryId.HasValue;
        }
    }
}