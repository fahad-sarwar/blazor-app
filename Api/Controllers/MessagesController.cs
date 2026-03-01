using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageRepository _messageRepository;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(MessageRepository messageRepository, ILogger<MessagesController> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

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

                var createdMessage = await _messageRepository.CreateMessage(message);

                return Ok(createdMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error saving message from {Email}.", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
