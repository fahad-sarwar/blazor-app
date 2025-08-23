using Api.Data;
using Api.Models;
using Api.Models.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController(OnlineShopContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWishlist([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null) return NotFound("Customer not found.");

            var wishlist = await context.Wishlist
                .Where(w => w.CustomerId == customer.Id)
                .OrderByDescending(w => w.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(w => w.Product)
                .ToListAsync();

            return Ok(wishlist.Select(w => new
            {
                w.Product.Id,
                w.Product.Name,
                w.Product.Description,
                w.Product.Price,
                w.Product.ImageURL,
                w.CreatedAt
            }));
        }

        [HttpGet("{productId}/exists")]
        public async Task<IActionResult> IsOnWishlist(int productId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null) return NotFound("Customer not found.");

            var exists = await context.Wishlist.AnyAsync(w => w.CustomerId == customer.Id && w.ProductId == productId);

            return exists ? Ok() : NotFound();
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToWishlist(AddToWishListRequest request)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null) return NotFound("Customer not found.");

            var product = await context.Product.FindAsync(request.ProductId);
            if (product == null) return NotFound("Product not found.");

            var existingWishlistItem = await context.Wishlist
                .FirstOrDefaultAsync(w => w.CustomerId == customer.Id && w.ProductId == request.ProductId);

            if (existingWishlistItem != null) return BadRequest("Product is already in the wishlist.");

            var wishlistItem = new Wishlist
            {
                CustomerId = customer.Id,
                ProductId = request.ProductId
            };

            context.Wishlist.Add(wishlistItem);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null) return NotFound("Customer not found.");

            var wishlistItem = await context.Wishlist
                .FirstOrDefaultAsync(w => w.CustomerId == customer.Id && w.ProductId == productId);

            if (wishlistItem == null) return NotFound("Product not found in wishlist.");

            context.Wishlist.Remove(wishlistItem);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
