using System.Security.Claims;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController(ProductRepository productRepository, CustomerRepository customerRepository, ILogger<WishlistController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWishlist([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var (products, totalCount) = await customerRepository.GetWishlistProducts(customer.Id, page, pageSize);

                return Ok(
                    new
                    {
                        Products = products,
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
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var exists = await customerRepository.IsProductInWishlist(customer.Id, productId);

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
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var product = await productRepository.GetProduct(request.ProductId);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                var exists = await customerRepository.IsProductInWishlist(customer.Id, request.ProductId);

                if (exists)
                {
                    return BadRequest("Product is already in the wishlist.");
                }

                await customerRepository.AddToWishlist(customer.Id, request.ProductId);

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
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var exists = await customerRepository.IsProductInWishlist(customer.Id, productId);

                if (!exists)
                {
                    return NotFound("Product not found in wishlist.");
                }

                await customerRepository.RemoveFromWishlist(customer.Id, productId);

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
