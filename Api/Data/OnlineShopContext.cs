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

        public DbSet<Api.Models.Db.Category> Category { get; set; } = default!;
        public DbSet<Api.Models.Db.Product> Product { get; set; } = default!;
        public DbSet<Api.Models.Db.Customer> Customer { get; set; } = default!;
        public DbSet<Api.Models.Db.Address> Address { get; set; } = default!;
        public DbSet<Api.Models.Db.Review> Review { get; set; } = default!;
        public DbSet<Api.Models.Db.Basket> Basket { get; set; } = default!;
        public DbSet<Api.Models.Db.BasketItem> BasketItem { get; set; } = default!;
    }
}
