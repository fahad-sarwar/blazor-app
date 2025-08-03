using Api.Data;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly OnlineShopContext _context;

        public BasketsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: api/Baskets
        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket([FromQuery] string anonymousUserId)
        {
            try
            {
                var basket = await _context.Basket
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
            var basket = await _context.Basket.FindAsync(id);

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

            _context.Entry(basket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.Basket.Add(basket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        // DELETE: api/Baskets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basket = await _context.Basket.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }

            _context.Basket.Remove(basket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasketExists(int id)
        {
            return _context.Basket.Any(e => e.Id == id);
        }
    }
}
