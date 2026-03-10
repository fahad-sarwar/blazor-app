using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly BasketRepository _basketRepository;
        private readonly ProductRepository _productRepository;
        private readonly TaxRateRepository _taxRateRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ILogger<BasketItemsController> _logger;

        public BasketItemsController(BasketRepository basketRepository, ProductRepository productRepository,
            TaxRateRepository taxRateRepository, CustomerRepository customerRepository, ILogger<BasketItemsController> logger)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _taxRateRepository = taxRateRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBasketItem(int id, UpdateBasketItemQuantityDTO updateBasketItemQuantity)
        {
            try
            {
                var exists = await _basketRepository.BasketItemExists(id);

                if (!exists)
                {
                    return NotFound();
                }

                await _basketRepository.UpdateBasketItemQuantity(id, updateBasketItemQuantity.Quantity);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating the quantity for the {BasketItemId} basket item.", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasketItem(CreateBasketItemDTO addBasketItem)
        {
            try
            {
                var taxRate = await _taxRateRepository.GetCurrentTaxRate();

                if (taxRate == null)
                {
                    return BadRequest("No tax rate found.");
                }

                var product = await _productRepository.GetProduct(addBasketItem.ProductId);

                if (product == null)
                {
                    return BadRequest("Product not found.");
                }

                if (addBasketItem.Quantity <= 0)
                {
                    return BadRequest("Quantity must be more than 0.");
                }

                if (string.IsNullOrEmpty(addBasketItem.AnonymousId) && !addBasketItem.CustomerId.HasValue)
                {
                    return BadRequest("The required account information was not provided.  Either provide an 'AnonymousId' or 'CustomerId'.");
                }

                if (addBasketItem.CustomerId.HasValue)
                {
                    var customer = await _customerRepository.GetCustomerById(addBasketItem.CustomerId.Value);

                    if (customer == null)
                    {
                        return BadRequest("Customer not found.");
                    }
                }

                var basket = await _basketRepository.GetOrCreateBasket(addBasketItem.AnonymousId, addBasketItem.CustomerId);

                if (basket == null)
                {
                    return BadRequest("Can't find or create basket.");
                }

                var basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    Product = product,
                    Quantity = addBasketItem.Quantity,
                    Price = product.ForSale ? product.SalePrice ?? product.Price : product.Price,
                    VATRate = taxRate.Rate,
                    CreatedAt = DateTime.UtcNow
                };

                var createdBasketItem = await _basketRepository.CreateBasketItem(basketItem);

                return Ok(createdBasketItem);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding item to basket");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem(int id)
        {
            try
            {
                var exists = await _basketRepository.BasketItemExists(id);

                if (!exists)
                {
                    return NotFound();
                }

                await _basketRepository.DeleteBasketItem(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error deleting basket item with id {BasketItemId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
