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
                new Product { Name = "Car Vacuum", Description = "Portable car vacuum cleaner.", Price = 29.99f, ImageURL = "", Stock = rnd.Next(10, 100), ForSale = rnd.Next(2) == 1, Category = categoryAutomotive },
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

            var customer1 = new Customer
            {
                UserName = "jdoe",
                FirstName = "John",
                LastName = "Doe",
                Email = "jdoe@example.com",
                PhoneNumber = "07123 456789",
                DateRegistered = DateTime.UtcNow.AddDays(-10),
                BillingAddress = address1Billing,
                ShippingAddress = address1Shipping
            };
            var customer2 = new Customer
            {
                UserName = "asmith",
                FirstName = "Alice",
                LastName = "Smith",
                Email = "asmith@example.com",
                PhoneNumber = "07234 567890",
                DateRegistered = DateTime.UtcNow.AddDays(-20),
                BillingAddress = address2Billing,
                ShippingAddress = address2Shipping
            };
            var customer3 = new Customer
            {
                UserName = "bwayne",
                FirstName = "Bruce",
                LastName = "Wayne",
                Email = "bwayne@example.com",
                PhoneNumber = "07345 678901",
                DateRegistered = DateTime.UtcNow.AddDays(-30),
                BillingAddress = address3Billing,
                ShippingAddress = address3Shipping
            };
            var customer4 = new Customer
            {
                UserName = "ckent",
                FirstName = "Clark",
                LastName = "Kent",
                Email = "ckent@example.com",
                PhoneNumber = "07456 789012",
                DateRegistered = DateTime.UtcNow.AddDays(-40),
                BillingAddress = address4Billing,
                ShippingAddress = address4Shipping
            };
            var customer5 = new Customer
            {
                UserName = "dprince",
                FirstName = "Diana",
                LastName = "Prince",
                Email = "dprince@example.com",
                PhoneNumber = "07567 890123",
                DateRegistered = DateTime.UtcNow.AddDays(-50),
                BillingAddress = address5Billing,
                ShippingAddress = address5Shipping
            };

            context.Customer.AddRange(customer1, customer2, customer3, customer4, customer5);
            context.SaveChanges();
        }
    }
}
