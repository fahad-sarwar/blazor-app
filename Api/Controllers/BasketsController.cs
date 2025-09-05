using Api.Data;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController(OnlineShopContext context) : ControllerBase
    {
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
    }
}
