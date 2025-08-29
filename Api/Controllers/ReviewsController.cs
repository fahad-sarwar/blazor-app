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

        // GET: api/Reviews
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

        // GET: api/Reviews/5
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

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, string status)
        {
            var review = await context.Review.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            if (!ReviewStatuses.Contains(status))
            {
                return BadRequest($"Invalid status. Allowed values are: {string.Join(", ", ReviewStatuses)}");
            }

            review.Status = status;

            context.Entry(review).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Reviews
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

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        private bool ReviewExists(int id)
        {
            return context.Review.Any(e => e.Id == id);
        }
    }
}
