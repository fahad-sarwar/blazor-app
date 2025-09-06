using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController(OnlineShopContext context, ILogger<BasketsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string anonymousUserId)
        {
            try
            {
                var basket = await context.Basket
                    .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                    .Where(b => b.AnonymousId == anonymousUserId)
                    .SingleOrDefaultAsync();

                return Ok(basket ?? new Basket());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving basket for AnonymousId: {AnonymousId}", anonymousUserId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
