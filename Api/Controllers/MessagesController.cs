using Api.Data;
using Api.Models;
using Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(OnlineShopContext context, ILogger<MessagesController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostBasket(SendMessageDTO request)
        {
            try
            {
                var message = new Message
                {
                    Name = request.Name,
                    Email = request.Email,
                    Subject = request.Subject,
                    Content = request.Content,
                };

                context.Message.Add(message);
                await context.SaveChangesAsync();

                return Ok(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving message from {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
