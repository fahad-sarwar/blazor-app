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

        public WishlistController(ProductRepository productRepository, CustomerRepository customerRepository)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            var customer = await GetCustomer();

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var products = await _customerRepository.GetWishlistProducts(customer.Id);

            return Ok(products);
        }

        [HttpGet("{productId}/exists")]
        public async Task<IActionResult> IsOnWishlist(int productId)
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

        [HttpPost("")]
        public async Task<IActionResult> AddToWishlist(AddToWishListDTO request)
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

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
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

        private async Task<Customer?> GetCustomer()
        {
            var email = User.FindFirst("Email")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _customerRepository.GetCustomerByEmail(email);
        }
    }
}
