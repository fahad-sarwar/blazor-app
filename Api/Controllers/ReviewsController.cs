using Api.Data;
using Api.Models;
using Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(OnlineShopContext context, ILogger<ReviewsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] int productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = context.Review
                    .Include(r => r.Customer)
                    .Where(r => r.Product.Id == productId)
                    .Where(r => r.Status == "Approved")
                    .OrderByDescending(r => r.CreatedAt)
                    .AsQueryable();

                var totalCount = await query.CountAsync();

                var paged = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(
                    new
                    {
                        Reviews = paged,
                        TotalCount = totalCount
                    }
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving reviews for product {ProductId}", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            try
            {
                var review = await context.Review
                    .Include(r => r.Customer)
                    .SingleOrDefaultAsync(r => r.Id == id);

                return review == null 
                    ? NotFound() 
                    : Ok(review);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving review with id {ReviewId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostReview(CreateReviewDTO request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                var product = await context.Product.FirstOrDefaultAsync(p => p.Id == request.ProductId);

                if (product == null)
                    return NotFound("Product not found.");

                var review = new Review
                {
                    Subject = request.Subject,
                    Rating = request.Rating,
                    Comment = request.Comment,
                    Status = "Pending",
                    Product = product,
                    Customer = customer,
                    CreatedAt = DateTime.UtcNow
                };

                context.Review.Add(review);
                await context.SaveChangesAsync();

                return Ok(review);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating review for product {ProductId}", request.ProductId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
