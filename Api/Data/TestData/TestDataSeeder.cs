using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Data.TestData
{
    public class TestDataSeeder
    {
        private static readonly Dictionary<string, Category> Categories = new();
        private static readonly List<Product> Products = [];
        private static readonly List<Customer> Customers = [];

        public static async Task SeedAsync(OnlineShopContext context, UserManager<ApplicationUser> userManager)
        {
            await ClearData(context);

            await BuildTaxRate(context);
            await BuildCategories(context);
            await BuildProducts(context);
            await BuildCustomers(context, userManager);
            await AddReviewsToProducts(context);
        }

        private static async Task ClearData(OnlineShopContext context)
        {
            context.Users.RemoveRange(context.Users);
            context.Wishlist.RemoveRange(context.Wishlist);
            context.Payment.RemoveRange(context.Payment);
            context.OrderTrackingUpdate.RemoveRange(context.OrderTrackingUpdate);
            context.OrderItem.RemoveRange(context.OrderItem);
            context.Order.RemoveRange(context.Order);
            context.BasketItem.RemoveRange(context.BasketItem);
            context.Basket.RemoveRange(context.Basket);
            context.Review.RemoveRange(context.Review);
            context.ProductAttribute.RemoveRange(context.ProductAttribute);
            context.Product.RemoveRange(context.Product);
            context.Category.RemoveRange(context.Category);
            context.Address.RemoveRange(context.Address);
            context.Customer.RemoveRange(context.Customer);
            context.TaxRate.RemoveRange(context.TaxRate);
            context.Message.RemoveRange(context.Message);
            await context.SaveChangesAsync();
        }

        private static async Task BuildTaxRate(OnlineShopContext context)
        {
            var taxRate = new TaxRate
            {
                Name = "VAT (20%)",
                Rate = 0.20,
                EffectiveFrom = DateTime.UtcNow.AddYears(-1),
            };

            context.TaxRate.Add(taxRate);
            await context.SaveChangesAsync();
        }

        private static async Task BuildCategories(OnlineShopContext context)
        {
            var rows = await File.ReadAllLinesAsync("Data/TestData/Data_Categories.txt");

            foreach (var row in rows.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    continue;
                }

                var columns = row.Split("###");

                if (columns.Length != 2)
                {
                    throw new InvalidOperationException($"Each category row must have at least 2 columns.  Row={row}");
                }

                var category = new Category
                {
                    Name = columns[0],
                    Description = columns[1],
                    CreatedAt = DateTime.UtcNow.AddYears(-1)
                };

                context.Category.Add(category);
                await context.SaveChangesAsync();

                Categories[category.Name] = category;
            }
        }

        private static async Task BuildProducts(OnlineShopContext context)
        {
            var rows = await File.ReadAllLinesAsync("Data/TestData/Data_Products.txt");

            foreach (var row in rows.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    continue;
                }

                var columns = row.Split("###");

                if (!(columns.Length == 26 || columns.Length == 28))
                {
                    throw new InvalidOperationException($"Each product row must have at least 26 or 28 columns.  ColumnCount={columns.Length}, Row={row}");
                }

                var isOnSale = columns[5] == "True";

                var product = new Product
                {
                    Name = columns[0],
                    Description = columns[1],
                    Price = double.Parse(columns[2]),
                    ImageURL = columns[3],
                    Stock = int.Parse(columns[4]),
                    ForSale = isOnSale,
                    SalePrice = isOnSale ? double.Parse(columns[6]) : null,
                    Category = Categories[columns[7]],
                    CreatedAt = DateTime.UtcNow.AddYears(-1)
                };

                product.Attributes.Add(await BuildProductAttribute(context, columns[8], columns[9]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[10], columns[11]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[12], columns[13]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[14], columns[15]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[16], columns[17]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[18], columns[19]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[20], columns[21]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[22], columns[23]));
                product.Attributes.Add(await BuildProductAttribute(context, columns[24], columns[25]));

                if(columns.Length == 28)
                    product.Attributes.Add(await BuildProductAttribute(context, columns[26], columns[27]));

                context.Product.Add(product);
                await context.SaveChangesAsync();

                Products.Add(product);
            }
        }

        private static async Task<ProductAttribute> BuildProductAttribute(OnlineShopContext context, string name, string value)
        {
            var productAttribute = new ProductAttribute
            {
                Name = name,
                Value = value
            };

            context.ProductAttribute.Add(productAttribute);
            await context.SaveChangesAsync();

            return productAttribute;
        }

        private static async Task BuildCustomers(OnlineShopContext context, UserManager<ApplicationUser> userManager)
        {
            var rows = await File.ReadAllLinesAsync("Data/TestData/Data_Customers.txt");

            foreach (var row in rows.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    continue;
                }

                var columns = row.Split("###");

                if (columns.Length != 17)
                {
                    throw new InvalidOperationException($"Each customer row must have at least 17 columns.  Row={row}");
                }

                var user = new ApplicationUser
                {
                    UserName = columns[2],
                    Email = columns[2],
                    PhoneNumber = columns[3],
                    FirstName = columns[0],
                    LastName = columns[1],
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "P@ssword1");

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create user {columns[2]}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                
                var shippingAddress = new Address
                {
                    AddressLineOne = columns[5],
                    AddressLineTwo = columns[6],
                    Town = columns[7],
                    County = columns[8],
                    PostCode= columns[9],
                    Country = columns[10],
                };

                context.Address.Add(shippingAddress);
                await context.SaveChangesAsync();

                var billingAddress = new Address
                {
                    AddressLineOne = columns[11],
                    AddressLineTwo = columns[12],
                    Town = columns[13],
                    County = columns[14],
                    PostCode = columns[15],
                    Country = columns[16],
                };

                context.Address.Add(billingAddress);
                await context.SaveChangesAsync();

                // TODO: Handle IsAdmin flag
                var isAdminUser = columns[4] == "1";

                var customer = new Customer
                {
                    FirstName = columns[0],
                    LastName = columns[1],
                    Email = columns[2],
                    PhoneNumber = columns[3],
                    UserId = user.Id,
                    User = user,
                    CreatedAt = DateTime.UtcNow.AddYears(-1)
                };

                context.Customer.Add(customer);
                await context.SaveChangesAsync();

                Customers.Add(customer);
            }
        }

        private static async Task AddReviewsToProducts(OnlineShopContext context)
        {
            var rnd = new Random();
            var reviews = new List<Review>();

            var rows = await File.ReadAllLinesAsync("Data/TestData/Data_Product_Reviews.txt");

            rows = rows
                .Where(r => !string.IsNullOrEmpty(r))
                .ToArray();

            foreach (var product in Products)
            {
                for (var i = 0; i < 2; i++)
                {
                    var customer = Customers[rnd.Next(Customers.Count)];
                    var randomIndex = rnd.Next(1, rows.Length); // TODO: make sure this doesn't error with "out of bounds" exception
                    var randomReview = rows[randomIndex];
                    var subject = randomReview.Split("###")[0];
                    var comment = randomReview.Split("###")[1];

                    var review = new Review
                    {
                        Subject = subject,
                        Rating = rnd.Next(1, 6), // 1 to 5
                        Comment = comment,
                        Status = "Approved",
                        Product = product,
                        Customer = customer,
                        CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(1, 100))
                    };

                    reviews.Add(review);
                }
            }

            context.Review.AddRange(reviews);
            await context.SaveChangesAsync();
        }
    }
}
