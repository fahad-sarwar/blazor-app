using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(MessageRepository messageRepository, ILogger<MessagesController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateMessage(SendMessageDTO request)
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

                var createdMessage = await messageRepository.CreateMessage(message);

                return Ok(createdMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving message from {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
