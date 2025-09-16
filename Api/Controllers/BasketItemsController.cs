using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController(IBasketItemRepository basketItemRepository, IBasketRepository basketRepository, IProductRepository productRepository,
        ITaxRateRepository taxRateRepository, ICustomerRepository customerRepository, ILogger<BasketItemsController> logger) : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBasketItem(int id, UpdateBasketItemQuantityDTO updateBasketItemQuantity)
        {
            try
            {
                var exists = await basketItemRepository.BasketItemExists(id);

                if (!exists)
                {
                    return NotFound();
                }

                await basketItemRepository.UpdateBasketItemQuantity(id, updateBasketItemQuantity.Quantity);

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error updating basket item with id {BasketItemId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasketItem(CreateBasketItemDTO addBasketItem)
        {
            try
            {
                var taxRate = await taxRateRepository.GetCurrentTaxRate();

                if (taxRate == null)
                {
                    return BadRequest("No valid tax rate found");
                }

                var product = await productRepository.GetProduct(addBasketItem.ProductId);

                if (product == null)
                {
                    return BadRequest("Product not found");
                }

                if (addBasketItem.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than zero");
                }

                if (string.IsNullOrEmpty(addBasketItem.AnonymousId) && !addBasketItem.CustomerId.HasValue)
                {
                    return BadRequest("Either AnonymousId or CustomerId must be provided");
                }

                if (addBasketItem.CustomerId.HasValue)
                {
                    var customer = await customerRepository.GetCustomerById(addBasketItem.CustomerId.Value);

                    if (customer == null)
                    {
                        return BadRequest("Customer not found");
                    }
                }

                var basket = await basketRepository.GetOrCreateBasket(addBasketItem.AnonymousId, addBasketItem.CustomerId);

                if (basket == null)
                {
                    return BadRequest("Unable to create or find basket");
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

                var createdBasketItem = await basketItemRepository.CreateBasketItem(basketItem);

                return Ok(createdBasketItem);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error adding basket item for product id {ProductId}", addBasketItem.ProductId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem(int id)
        {
            try
            {
                var exists = await basketItemRepository.BasketItemExists(id);

                if (!exists)
                {
                    return NotFound();
                }

                await basketItemRepository.DeleteBasketItem(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting basket item with id {BasketItemId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
