using Api.Data;
using Api.Models;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(OnlineShopContext context) : ControllerBase
    {
        private static readonly string[] ReviewStatuses = new[] { "Pending", "Approved", "Rejected" };

        [HttpGet]
        public async Task<ActionResult<PagedReviewResult>> GetReviews([FromQuery] int productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

            return new PagedReviewResult()
            {
                Reviews = paged,
                TotalCount = totalCount
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await context.Review
                .Include(r => r.Customer)
                .SingleOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(CreateReviewRequest request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return NotFound("Customer not found.");

            var product = await context.Product.FirstOrDefaultAsync(p => p.Id == request.ProductId);
            if (product == null) return NotFound("Product not found.");

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

            return review;
        }
    }
}
