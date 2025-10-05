using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class ProductRepository : RepositoryBase
    {
        public async Task<(List<Product> Products, int TotalCount)> GetProducts(int? categoryId, bool? forSale, string? searchTerm, string sort, int page, int pageSize)
        {
            var products = new List<Product>();
            var queryWhereConditions = new List<string>();
            var commandParameters = new List<(string name, object value)>();

            if (categoryId.HasValue)
            {
                queryWhereConditions.Add("p.CategoryId = @categoryId");
                commandParameters.Add(("@categoryId", categoryId.Value));
            }

            if (forSale.HasValue)
            {
                queryWhereConditions.Add("p.ForSale = @forSale");
                commandParameters.Add(("@forSale", forSale.Value));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                queryWhereConditions.Add("(LOWER(p.Name) LIKE @searchTerm OR LOWER(p.Description) LIKE @searchTerm)");
                commandParameters.Add(("@searchTerm", $"%{searchTerm.ToLower()}%"));
            }

            var whereClause = queryWhereConditions.Count > 0 
                ? "WHERE " + string.Join(" AND ", queryWhereConditions)
                : "";

            string orderBy;
            switch(sort)
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
                "SELECT p.Id, p.Name, p.Description, p.Price, p.ImageURL, p.Stock, p.ForSale, p.SalePrice, p.CreatedAt, c.Id, c.Name, c.Description, c.CreatedAt " +
                "FROM Product p " +
                " INNER JOIN Category c ON p.CategoryId = c.Id " +
                $"{whereClause} " +
                $"{orderBy} " +
                "LIMIT @pageSize OFFSET @offset";

            await using var conn = new SqliteConnection(ConnectionString);            
            try
            {
                conn.Open();

                await using var countCommand = new SqliteCommand(countQuery, conn);

                var countParameters = new Dictionary<string, object>();

                foreach (var param in commandParameters)
                {
                    countParameters.Add(param.name, param.value);
                }

                var totalCount = await ExecuteScalar(countQuery, countParameters);

                commandParameters.Add(("@pageSize", pageSize));
                commandParameters.Add(("@offset", (page - 1) * pageSize));

                await using var dataCommand = new SqliteCommand(dataQuery, conn);
                foreach (var param in commandParameters)
                {
                    dataCommand.Parameters.AddWithValue(param.name, param.value);
                }

                var reader = await dataCommand.ExecuteReaderAsync();

                while (reader.Read())
                {
                    var product = new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDouble(3),
                        ImageURL = reader.GetString(4),
                        Stock = reader.GetInt32(5),
                        ForSale = reader.GetBoolean(6),
                        SalePrice = reader.IsDBNull(7) ? null : reader.GetDouble(7),
                        CreatedAt = reader.GetDateTime(8),
                        Category = new Category
                        {
                            Id = reader.GetInt32(9),
                            Name = reader.GetString(10),
                            Description = reader.GetString(11),
                            CreatedAt = reader.GetDateTime(12)
                        }
                    };

                    products.Add(product);
                }

                return (products, totalCount);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Product?> GetProduct(int productId)
        {
            Product? product = null;

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
            try
            {
                conn.Open();

                await using var productCommand = new SqliteCommand(query, conn);
                productCommand.Parameters.AddWithValue("@productId", productId);

                var reader = await productCommand.ExecuteReaderAsync();

                if (reader.Read())
                {
                    product = new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDouble(3),
                        ImageURL = reader.GetString(4),
                        Stock = reader.GetInt32(5),
                        ForSale = reader.GetBoolean(6),
                        SalePrice = reader.IsDBNull(7) ? null : reader.GetDouble(7),
                        CreatedAt = reader.GetDateTime(8),
                        Category = new Category
                        {
                            Id = reader.GetInt32(9),
                            Name = reader.GetString(10),
                            Description = reader.GetString(11),
                            CreatedAt = reader.GetDateTime(12)
                        },
                        Attributes = new List<ProductAttribute>()
                    };
                }

                reader.Close();

                if (product != null)
                {
                    await using var attributesCommand = new SqliteCommand(attributesQuery, conn);
                    attributesCommand.Parameters.AddWithValue("@productId", productId);

                    var attributesReader = await attributesCommand.ExecuteReaderAsync();

                    while (attributesReader.Read())
                    {
                        product.Attributes.Add(new ProductAttribute
                        {
                            Id = attributesReader.GetInt32(0),
                            Name = attributesReader.GetString(1),
                            Value = attributesReader.GetString(2)
                        });
                    }
                }

                return product;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}