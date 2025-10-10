using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class ProductRepository : RepositoryBase
    {
        public async Task<(List<Product> Products, int TotalCount)> GetProducts(int? categoryId, bool? forSale, string? searchTerm, string sort, int page, int pageSize)
        {
            var queryWhereConditions = new List<string>();
            var commandParameters = new DynamicParameters();

            if (categoryId.HasValue)
            {
                queryWhereConditions.Add("p.CategoryId = @categoryId");
                commandParameters.Add("@categoryId", categoryId.Value);
            }

            if (forSale.HasValue)
            {
                queryWhereConditions.Add("p.ForSale = @forSale");
                commandParameters.Add("@forSale", forSale.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                queryWhereConditions.Add("(LOWER(p.Name) LIKE @searchTerm OR LOWER(p.Description) LIKE @searchTerm)");
                commandParameters.Add("@searchTerm", $"%{searchTerm.ToLower()}%");
            }

            var whereClause = queryWhereConditions.Count > 0
                ? "WHERE " + string.Join(" AND ", queryWhereConditions)
                : "";

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

            var countQuery =
                "SELECT COUNT(*) " +
                "FROM Product p " +
                "INNER JOIN Category c ON p.CategoryId = c.Id " +
                $"{whereClause} ";

            var dataQuery =
                "SELECT p.Id, p.Name, p.Description, p.Price, p.ImageURL, p.Stock, p.ForSale, p.SalePrice, p.CreatedAt, c.Id as CategoryId, c.Name, c.Description, c.CreatedAt " +
                "FROM Product p " +
                " INNER JOIN Category c ON p.CategoryId = c.Id " +
                $"{whereClause} " +
                $"{orderBy} " +
                "LIMIT @pageSize OFFSET @offset";

            commandParameters.Add("pageSize", pageSize);
            commandParameters.Add("offset", (page - 1) * pageSize);

            await using var conn = new SqliteConnection(ConnectionString);

            var totalCount = await conn.QuerySingleAsync<int>(countQuery, commandParameters);

            var productData = await conn.QueryAsync<Product, Category, Product>(
                dataQuery,
                (product, category) =>
                {
                    product.Category = category;
                    return product;
                },
                commandParameters,
                splitOn: "CategoryId"
            );

            return (productData.ToList(), totalCount);
        }

        public async Task<Product?> GetProduct(int productId)
        {
            var query =
                "SELECT p.Id, p.Name, p.Description, p.Price, p.ImageURL, p.Stock, p.ForSale, p.SalePrice, p.CreatedAt, " +
                "c.Id as CategoryId, c.Name, c.Description, c.CreatedAt " +
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
                splitOn: "CategoryId"
            );

            var result = product.FirstOrDefault();

            if (result != null)
            {
                var attributes = await conn.QueryAsync<ProductAttribute>(attributesQuery, new { productId });
                result.Attributes = attributes.ToList();
            }

            return result;
        }
    }
}