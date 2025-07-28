using Api.Data;
using Api.Models.Db;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly OnlineShopContext _context;

        public BasketItemsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: api/BasketItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetBasketItem()
        {
            return await _context.BasketItem.ToListAsync();
        }

        // GET: api/BasketItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketItem>> GetBasketItem(int id)
        {
            var basketItem = await _context.BasketItem.FindAsync(id);

            if (basketItem == null)
            {
                return NotFound();
            }

            return basketItem;
        }

        // PUT: api/BasketItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasketItem(int id, UpdateBasketItemQuantity updateBasketItemQuantity)
        {
            var basketItem = await _context.BasketItem.FindAsync(id);

            if (basketItem == null)
            {
                return NotFound();
            }

            basketItem.Quantity = updateBasketItemQuantity.Quantity;

            _context.Entry(basketItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BasketItems
        [HttpPost]
        public async Task<ActionResult<BasketItem>> PostBasketItem(AddBasketItem addBasketItem)
        {
            var product = await _context.Product.SingleOrDefaultAsync(p => p.Id == addBasketItem.ProductId);

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

            var customer = await _context.Customer.SingleOrDefaultAsync(c => c.Id == addBasketItem.CustomerId);

            if (addBasketItem.CustomerId.HasValue && customer == null)
            {
                return BadRequest("Customer not found");
            }

            var basket = await _context.Basket
                .SingleOrDefaultAsync(b => b.AnonymousId == addBasketItem.AnonymousId ||
                (b.Customer != null && b.Customer.Id == addBasketItem.CustomerId));

            if (basket == null)
            {
                basket = new Basket
                {
                    AnonymousId = addBasketItem.AnonymousId,
                    Customer = addBasketItem.CustomerId.HasValue ? await _context.Customer.FindAsync(addBasketItem.CustomerId.Value) : null
                };
                _context.Basket.Add(basket);
                await _context.SaveChangesAsync();
            }

            var basketItem = new BasketItem
            {
                BasketId = basket.Id,
                Product = product,
                Quantity = addBasketItem.Quantity,
                Price = product.Price,
                AddedAt = DateTime.UtcNow
            };

            _context.BasketItem.Add(basketItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasketItem", new { id = basketItem.Id }, basketItem);
        }

        // DELETE: api/BasketItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem(int id)
        {
            var basketItem = await _context.BasketItem.FindAsync(id);

            if (basketItem == null)
            {
                return NotFound();
            }

            _context.BasketItem.Remove(basketItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasketItemExists(int id)
        {
            return _context.BasketItem.Any(e => e.Id == id);
        }
    }
}
