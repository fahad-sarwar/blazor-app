using System.Security.Claims;
using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ILogger<WishlistController> _logger;

        public WishlistController(ProductRepository productRepository, CustomerRepository customerRepository, ILogger<WishlistController> logger)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var customer = await GetCustomer();

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var (products, totalCount) = await _customerRepository.GetWishlistProducts(customer.Id, page, pageSize);

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
                _logger.LogError(ex, "There was an error getting the customers wishlist.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{productId}/exists")]
        public async Task<IActionResult> IsOnWishlist(int productId)
        {
            try
            {
                var customer = await GetCustomer();

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var exists = await _customerRepository.IsProductInWishlist(customer.Id, productId);

                return exists
                    ? Ok()
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error checking if the product with id of {Product} is on the wishlist.", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToWishlist(AddToWishListDTO request)
        {
            try
            {
                var customer = await GetCustomer();

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var product = await _productRepository.GetProduct(request.ProductId);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                var exists = await _customerRepository.IsProductInWishlist(customer.Id, request.ProductId);

                if (exists)
                {
                    return BadRequest("Already on wishlist.");
                }

                await _customerRepository.AddToWishlist(customer.Id, request.ProductId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error adding product with id {Product} to the customers wishlist.", request.ProductId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            try
            {
                var customer = await GetCustomer();

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var exists = await _customerRepository.IsProductInWishlist(customer.Id, productId);

                if (!exists)
                {
                    return NotFound("Not on wishlist.");
                }

                await _customerRepository.RemoveFromWishlist(customer.Id, productId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error removing the product was id {Product} from the customers wishlist.", productId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<Customer?> GetCustomer()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _customerRepository.GetCustomerByEmail(email);
        }
    }
}
