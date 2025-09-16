using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController(IBasketRepository basketRepository, ILogger<BasketsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string anonymousUserId)
        {
            try
            {
                var basket = await basketRepository.GetBasketByAnonymousId(anonymousUserId);

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
