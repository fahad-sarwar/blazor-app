using Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class OnlineShopContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineShopContext(DbContextOptions<OnlineShopContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<ProductAttribute> ProductAttribute { get; set; } = default!;
        public DbSet<Customer> Customer { get; set; } = default!;
        public DbSet<Address> Address { get; set; } = default!;
        public DbSet<Review> Review { get; set; } = default!;
        public DbSet<Basket> Basket { get; set; } = default!;
        public DbSet<BasketItem> BasketItem { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<OrderItem> OrderItem { get; set; } = default!;
        public DbSet<OrderTrackingUpdate> OrderTrackingUpdate { get; set; } = default!;
        public DbSet<Payment> Payment { get; set; } = default!;
        public DbSet<TaxRate> TaxRate { get; set; } = default!;
        public DbSet<Wishlist> Wishlist { get; set; } = default!;
        public DbSet<Message> Message { get; set; } = default!;
    }
}
