using System.Security.Claims;
using Api.Data;
using Api.Models;
using Api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController(OnlineShopContext context, ILogger<WishlistController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWishlist([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                var query = context.Wishlist
                    .Where(w => w.CustomerId == customer.Id)
                    .OrderByDescending(w => w.CreatedAt)
                    .AsQueryable();

                var totalCount = await query.CountAsync();

                var paged = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include(w => w.Product)
                    .ToListAsync();

                return Ok(
                    new
                    {
                        Products = paged.Select(w => w.Product).ToList(),
                        TotalCount = totalCount
                    }
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving wishlist");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{productId}/exists")]
        public async Task<IActionResult> IsOnWishlist(int productId)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                var exists = await context.Wishlist.AnyAsync(w => w.CustomerId == customer.Id && w.ProductId == productId);

                return exists 
                    ? Ok() 
                    : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking wishlist for product {ProductId}", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToWishlist(AddToWishListDTO request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                var product = await context.Product.FindAsync(request.ProductId);

                if (product == null)
                    return NotFound("Product not found.");

                var existingWishlistItem = await context.Wishlist
                    .FirstOrDefaultAsync(w => w.CustomerId == customer.Id && w.ProductId == request.ProductId);

                if (existingWishlistItem != null)
                    return BadRequest("Product is already in the wishlist.");

                var wishlistItem = new Wishlist
                {
                    CustomerId = customer.Id,
                    ProductId = request.ProductId
                };

                context.Wishlist.Add(wishlistItem);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding product {ProductId} to wishlist", request.ProductId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                var wishlistItem = await context.Wishlist
                    .FirstOrDefaultAsync(w => w.CustomerId == customer.Id && w.ProductId == productId);

                if (wishlistItem == null)
                    return NotFound("Product not found in wishlist.");

                context.Wishlist.Remove(wishlistItem);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing product {ProductId} from wishlist", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
