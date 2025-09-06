using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models.DTOs;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController(OnlineShopContext context, ILogger<BasketItemsController> logger) : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasketItem(int id, UpdateBasketItemQuantityDTO updateBasketItemQuantity)
        {
            try
            {
                var basketItem = await context.BasketItem.FindAsync(id);

                if (basketItem == null)
                    return NotFound();

                basketItem.Quantity = updateBasketItemQuantity.Quantity;

                context.Entry(basketItem).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketItemExists(id))
                        return NotFound();

                    throw;
                }

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error updating basket item with id {BasketItemId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostBasketItem(CreateBasketItemDTO addBasketItem)
        {
            try
            {
                var product = await context.Product.SingleOrDefaultAsync(p => p.Id == addBasketItem.ProductId);

                if (product == null)
                    return BadRequest("Product not found");

                if (addBasketItem.Quantity <= 0)
                    return BadRequest("Quantity must be greater than zero");

                if (string.IsNullOrEmpty(addBasketItem.AnonymousId) && !addBasketItem.CustomerId.HasValue)
                    return BadRequest("Either AnonymousId or CustomerId must be provided");

                var customer = await context.Customer.SingleOrDefaultAsync(c => c.Id == addBasketItem.CustomerId);

                if (addBasketItem.CustomerId.HasValue && customer == null)
                    return BadRequest("Customer not found");

                var basket = await context.Basket
                    .SingleOrDefaultAsync(b => b.AnonymousId == addBasketItem.AnonymousId ||
                                               (b.Customer != null && b.Customer.Id == addBasketItem.CustomerId));

                if (basket == null)
                {
                    basket = new Basket
                    {
                        AnonymousId = addBasketItem.AnonymousId,
                        Customer = addBasketItem.CustomerId.HasValue ? await context.Customer.FindAsync(addBasketItem.CustomerId.Value) : null
                    };
                    context.Basket.Add(basket);
                    await context.SaveChangesAsync();
                }

                var basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    Product = product,
                    Quantity = addBasketItem.Quantity,
                    Price = product.Price,
                    CreatedAt = DateTime.UtcNow
                };

                context.BasketItem.Add(basketItem);
                await context.SaveChangesAsync();

                return Ok(basketItem);
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
                var basketItem = await context.BasketItem.FindAsync(id);

                if (basketItem == null)
                    return NotFound();

                context.BasketItem.Remove(basketItem);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting basket item with id {BasketItemId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool BasketItemExists(int id)
        {
            return context.BasketItem.Any(e => e.Id == id);
        }
    }
}
