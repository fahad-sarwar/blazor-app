using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly BasketRepository _basketRepository;
        private readonly ILogger<BasketsController> _logger;

        public BasketsController(BasketRepository basketRepository, ILogger<BasketsController> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string anonymousUserId)
        {
            try
            {
                var basket = await _basketRepository.GetBasketByAnonymousId(anonymousUserId);

                return Ok(basket ?? new Basket());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error retrieving the basket for AnonymousId: {AnonymousId}.", anonymousUserId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
