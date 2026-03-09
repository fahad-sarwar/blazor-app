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

        public MessagesController(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(SendMessageDTO request)
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
    }
}
