using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(CustomerRepository customerRepository, ProductRepository productRepository, ILogger<ReviewsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] int productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (reviews, totalCount) = await customerRepository.GetReviews(productId, page, pageSize);

                return Ok(
                    new
                    {
                        Reviews = reviews,
                        TotalCount = totalCount
                    }
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error getting reviews for product with id {Product}.", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetReviewStats([FromQuery] int productId)
        {
            try
            {
                var averageRating = await customerRepository.GetAverageRating(productId);

                return Ok(
                    new
                    {
                        AverageRating = averageRating
                    }
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error getting product review stats for product with id {Product}", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDTO request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized("The user was not found.  Please ensure the customer is logged in.");
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("The customer was not found.  Please ensure the customer is logged in.");
                }

                var product = await productRepository.GetProduct(request.ProductId);

                if (product == null)
                {
                    return NotFound("The specified product was not found.  Please enter the correct product.");
                }

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

                var createdReview = await customerRepository.CreateReview(review);

                return Ok(createdReview);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error creating a review for product with id {Product}.", request.ProductId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
