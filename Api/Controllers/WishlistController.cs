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
                    return Unauthorized("The user was not found.  Please ensure the customer is logged in.");
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("The customer was not found.  Please provide the correct details.");
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
                logger.LogError(ex, "There was an error getting the customers wishlist.");
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
                    return Unauthorized("The user was not found.  Please ensure the customer is logged in.");
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("The customer was not found.  Please ensure the customer is logged in.");
                }

                var exists = await customerRepository.IsProductInWishlist(customer.Id, productId);

                return exists 
                    ? Ok() 
                    : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error checking if the product with id of {Product} is on the wishlist.", productId);
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
                    return NotFound("The entered product was not found.  Please provide the correct details.");
                }

                var exists = await customerRepository.IsProductInWishlist(customer.Id, request.ProductId);

                if (exists)
                {
                    return BadRequest("The product is already on the customers wishlist.  Please try again with another product.");
                }

                await customerRepository.AddToWishlist(customer.Id, request.ProductId);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error adding product with id {Product} to the customers wishlist.", request.ProductId);
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
                    return Unauthorized("The user was not found.  Please ensure the customer is logged in.");
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("The customer was not found.  Please ensure the customer is logged in.");
                }

                var exists = await customerRepository.IsProductInWishlist(customer.Id, productId);

                if (!exists)
                {
                    return NotFound("The specified product is not on the customers wishlist.");
                }

                await customerRepository.RemoveFromWishlist(customer.Id, productId);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error removing the product was id {Product} from the customers wishlist.", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
