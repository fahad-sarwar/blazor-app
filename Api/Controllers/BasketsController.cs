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

        public BasketsController(BasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string anonymousUserId)
        {
            var basket = await _basketRepository.GetBasketByAnonymousId(anonymousUserId);

            return Ok(basket ?? new Basket());
        }
    }
}
