using Api.Data;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController(OnlineShopContext context) : ControllerBase
    {
        // GET: api/Baskets
        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket([FromQuery] string anonymousUserId)
        {
            try
            {
                var basket = await context.Basket
                    .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                    .Where(b => b.AnonymousId == anonymousUserId)
                    .SingleOrDefaultAsync();

                return basket ?? new Basket();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }

        // GET: api/Baskets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> GetBasket(int id)
        {
            var basket = await context.Basket.FindAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            return basket;
        }

        // PUT: api/Baskets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasket(int id, Basket basket)
        {
            if (id != basket.Id)
            {
                return BadRequest();
            }

            context.Entry(basket).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketExists(id))
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

        // POST: api/Baskets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Basket>> PostBasket(Basket basket)
        {
            context.Basket.Add(basket);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        // DELETE: api/Baskets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basket = await context.Basket.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }

            context.Basket.Remove(basket);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasketExists(int id)
        {
            return context.Basket.Any(e => e.Id == id);
        }
    }
}
