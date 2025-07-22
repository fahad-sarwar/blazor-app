using Api.Models.Db;

namespace Api.Data.TestData
{
    public class TestDataSeeder
    {
        public static void Seed(OnlineShopContext context)
        {
            context.Product.RemoveRange(context.Product);
            context.Category.RemoveRange(context.Category);
            context.Address.RemoveRange(context.Address);
            context.Customer.RemoveRange(context.Customer);
            context.Review.RemoveRange(context.Review);
            context.SaveChanges();

            var categoryAutomotive = new Category { Name = "Automotive", Description = "Car accessories, tools, and parts." };
            var categoryBooks = new Category { Name = "Books", Description = "Printed and digital books across genres." };
            var categoryClothing = new Category { Name = "Clothing", Description = "Men's, women's, and children's apparel." };
            var categoryElectronics = new Category { Name = "Electronics", Description = "Devices, gadgets, and accessories." };
            var categorySportsOutdoors = new Category { Name = "Sports & Outdoors", Description = "Equipment and gear for sports and outdoor activities." };
            var categoryToysGames = new Category { Name = "Toys & Games", Description = "Toys, puzzles, and games for all ages." };

            context.Category.AddRange(
                categoryAutomotive,
                categoryBooks,
                categoryClothing,
                categoryElectronics,
                categorySportsOutdoors,
                categoryToysGames
            );
            context.SaveChanges();

            var rnd = new Random();
            var products = new List<Product>
            {
                // Automotive
                new Product { Name = "Car Vacuum", Description = "Portable car vacuum cleaner.", Price = 29.99f, ImageURL = "data:image/jpeg;base64,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", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Dash Cam", Description = "Full HD dashboard camera.", Price = 59.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Car Cover", Description = "All-weather car cover.", Price = 49.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Tire Inflator", Description = "Digital portable inflator.", Price = 34.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Seat Organizer", Description = "Multi-pocket car seat organizer.", Price = 19.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Steering Wheel Cover", Description = "Leather steering wheel cover.", Price = 15.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Car Air Freshener", Description = "Long-lasting fresh scent.", Price = 7.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Jump Starter", Description = "Portable car jump starter.", Price = 69.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "Windshield Sun Shade", Description = "UV protection sun shade.", Price = 12.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
                new Product { Name = "LED Headlights", Description = "Bright white LED bulbs.", Price = 39.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },

                // Books
                new Product { Name = "Mystery Novel", Description = "A thrilling mystery by Jane Doe.", Price = 14.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Science Textbook", Description = "Comprehensive science reference.", Price = 59.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Cookbook", Description = "Delicious recipes from around the world.", Price = 24.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Children's Storybook", Description = "Illustrated bedtime stories.", Price = 9.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Travel Guide", Description = "Explore Europe with this guide.", Price = 19.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Fantasy Epic", Description = "An epic fantasy adventure.", Price = 17.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Biography", Description = "Life story of a famous figure.", Price = 21.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Self-Help Book", Description = "Improve your life with these tips.", Price = 12.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Art Book", Description = "A collection of modern art.", Price = 34.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                new Product { Name = "Programming Guide", Description = "Learn C# with practical examples.", Price = 29.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryBooks },
                
                // Clothing
                new Product { Name = "Men's T-Shirt", Description = "100% cotton, various sizes.", Price = 14.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Women's Jeans", Description = "Slim fit, blue denim.", Price = 39.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Children's Jacket", Description = "Warm and waterproof.", Price = 29.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Sneakers", Description = "Comfortable running shoes.", Price = 49.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Baseball Cap", Description = "Adjustable, various colors.", Price = 12.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Dress Shirt", Description = "Formal wear, white.", Price = 24.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Summer Dress", Description = "Lightweight, floral print.", Price = 34.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Wool Scarf", Description = "Soft and warm.", Price = 19.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Leather Belt", Description = "Genuine leather, brown.", Price = 22.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },
                new Product { Name = "Socks (5-pack)", Description = "Cotton blend, assorted colors.", Price = 9.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryClothing },

                // Electronics
                new Product { Name = "Smartphone X1", Description = "Latest 5G smartphone with OLED display.", Price = 799.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "Bluetooth Headphones", Description = "Noise-cancelling over-ear headphones.", Price = 199.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "4K TV", Description = "Ultra HD Smart TV, 55-inch.", Price = 599.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "Wireless Mouse", Description = "Ergonomic wireless mouse.", Price = 29.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "Gaming Laptop", Description = "High-performance laptop for gaming.", Price = 1299.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "Smartwatch", Description = "Fitness tracking smartwatch.", Price = 149.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "Tablet Pro", Description = "10-inch tablet with stylus.", Price = 399.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "Bluetooth Speaker", Description = "Portable waterproof speaker.", Price = 49.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "Action Camera", Description = "4K waterproof action camera.", Price = 249.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },
                new Product { Name = "E-Reader", Description = "6-inch e-ink display e-reader.", Price = 89.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryElectronics },

                // Sports & Outdoors
                new Product { Name = "Yoga Mat", Description = "Non-slip exercise mat.", Price = 19.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Mountain Bike", Description = "21-speed mountain bike.", Price = 299.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Tennis Racket", Description = "Lightweight graphite racket.", Price = 59.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Camping Tent", Description = "4-person waterproof tent.", Price = 129.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Sleeping Bag", Description = "All-season sleeping bag.", Price = 49.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Dumbbell Set", Description = "Adjustable dumbbells, 40 lbs.", Price = 79.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Soccer Ball", Description = "Official size and weight.", Price = 24.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Hiking Backpack", Description = "50L waterproof backpack.", Price = 69.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Swim Goggles", Description = "Anti-fog UV protection.", Price = 14.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },
                new Product { Name = "Fitness Tracker", Description = "Tracks steps, calories, and sleep.", Price = 49.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categorySportsOutdoors },

                // Toys & Games
                new Product { Name = "Building Blocks", Description = "100-piece colorful blocks.", Price = 24.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Remote Control Car", Description = "High-speed RC car.", Price = 39.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Puzzle Set", Description = "500-piece jigsaw puzzle.", Price = 14.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Board Game", Description = "Strategy board game for families.", Price = 29.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Dollhouse", Description = "Wooden dollhouse with furniture.", Price = 59.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Action Figure", Description = "Superhero action figure.", Price = 12.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Toy Train Set", Description = "Battery-operated train set.", Price = 34.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Plush Bear", Description = "Soft and cuddly teddy bear.", Price = 19.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Kite", Description = "Easy-fly rainbow kite.", Price = 9.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
                new Product { Name = "Marble Run", Description = "Creative marble run set.", Price = 27.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryToysGames },
            };

            context.Product.AddRange(products);

            var address1Billing = new Address
            {
                AddressLineOne = "10 Downing Street",
                AddressLineTwo = "",
                Town = "London",
                County = "Greater London",
                PostCode = "SW1A 2AA",
                Country = "UK"
            };
            var address1Shipping = new Address
            {
                AddressLineOne = "221B Baker Street",
                AddressLineTwo = "",
                Town = "London",
                County = "Greater London",
                PostCode = "NW1 6XE",
                Country = "UK"
            };

            var address2Billing = new Address
            {
                AddressLineOne = "1 Castle Street",
                AddressLineTwo = "",
                Town = "Edinburgh",
                County = "Midlothian",
                PostCode = "EH1 2ND",
                Country = "UK"
            };
            var address2Shipping = new Address
            {
                AddressLineOne = "50 George Square",
                AddressLineTwo = "Flat 2",
                Town = "Edinburgh",
                County = "Midlothian",
                PostCode = "EH8 9JU",
                Country = "UK"
            };

            var address3Billing = new Address
            {
                AddressLineOne = "100 King Street",
                AddressLineTwo = "",
                Town = "Manchester",
                County = "Greater Manchester",
                PostCode = "M2 4WU",
                Country = "UK"
            };
            var address3Shipping = new Address
            {
                AddressLineOne = "200 Deansgate",
                AddressLineTwo = "Apt 5B",
                Town = "Manchester",
                County = "Greater Manchester",
                PostCode = "M3 3NN",
                Country = "UK"
            };

            var address4Billing = new Address
            {
                AddressLineOne = "1 Queen Square",
                AddressLineTwo = "",
                Town = "Bristol",
                County = "Bristol",
                PostCode = "BS1 4JQ",
                Country = "UK"
            };
            var address4Shipping = new Address
            {
                AddressLineOne = "15 Park Street",
                AddressLineTwo = "",
                Town = "Bristol",
                County = "Bristol",
                PostCode = "BS1 5HX",
                Country = "UK"
            };

            var address5Billing = new Address
            {
                AddressLineOne = "30 Bold Street",
                AddressLineTwo = "",
                Town = "Liverpool",
                County = "Merseyside",
                PostCode = "L1 4DS",
                Country = "UK"
            };
            var address5Shipping = new Address
            {
                AddressLineOne = "8 Hope Street",
                AddressLineTwo = "",
                Town = "Liverpool",
                County = "Merseyside",
                PostCode = "L1 9BX",
                Country = "UK"
            };

            context.Address.AddRange(
                address1Billing, address1Shipping,
                address2Billing, address2Shipping,
                address3Billing, address3Shipping,
                address4Billing, address4Shipping,
                address5Billing, address5Shipping
            );
            context.SaveChanges();

            var customers = new List<Customer>
            {
                new Customer
                {
                    UserName = "jdoe",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "jdoe@example.com",
                    PhoneNumber = "07123 456789",
                    DateRegistered = DateTime.UtcNow.AddDays(-10),
                    BillingAddress = address1Billing,
                    ShippingAddress = address1Shipping
                },
                new Customer
                {
                    UserName = "asmith",
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "asmith@example.com",
                    PhoneNumber = "07234 567890",
                    DateRegistered = DateTime.UtcNow.AddDays(-20),
                    BillingAddress = address2Billing,
                    ShippingAddress = address2Shipping
                },
                new Customer
                {
                    UserName = "bwayne",
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    Email = "bwayne@example.com",
                    PhoneNumber = "07345 678901",
                    DateRegistered = DateTime.UtcNow.AddDays(-30),
                    BillingAddress = address3Billing,
                    ShippingAddress = address3Shipping
                },
                new Customer
                {
                    UserName = "ckent",
                    FirstName = "Clark",
                    LastName = "Kent",
                    Email = "ckent@example.com",
                    PhoneNumber = "07456 789012",
                    DateRegistered = DateTime.UtcNow.AddDays(-40),
                    BillingAddress = address4Billing,
                    ShippingAddress = address4Shipping
                },
                new Customer
                {
                    UserName = "dprince",
                    FirstName = "Diana",
                    LastName = "Prince",
                    Email = "dprince@example.com",
                    PhoneNumber = "07567 890123",
                    DateRegistered = DateTime.UtcNow.AddDays(-50),
                    BillingAddress = address5Billing,
                    ShippingAddress = address5Shipping
                }
            };

            context.Customer.AddRange(customers);

            var reviewComments = new[]
            {
                "Excellent product!",
                "Very satisfied.",
                "Would buy again.",
                "Not as expected.",
                "Good value for money.",
                "Quality could be better.",
                "Fast delivery.",
                "Highly recommended.",
                "Average experience.",
                "Superb!"
            };

            var reviews = new List<Review>();

            foreach (var product in products)
            {
                for (int i = 0; i < 2; i++)
                {
                    var customer = customers[rnd.Next(customers.Count)];
                    var review = new Review
                    {
                        ReviewerName = $"{customer.FirstName} {customer.LastName}",
                        Rating = rnd.Next(1, 6), // 1 to 5
                        Comment = reviewComments[rnd.Next(reviewComments.Length)],
                        Product = product,
                        Customer = customer,
                        DateCreated = DateTime.UtcNow.AddDays(-rnd.Next(1, 100))
                    };
                    reviews.Add(review);
                }
            }

            context.Review.AddRange(reviews);

            context.SaveChanges();
        }
    }
}
