using Api.Data;
using Api.Models;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(OnlineShopContext context) : ControllerBase
    {

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await context.Message.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        [HttpPost]
        public async Task<ActionResult<Message>> PostBasket(CreateMessageRequest request)
        {
            var message = new Message()
            {
                Name = request.Name,
                Email = request.Email,
                Subject = request.Subject,
                Content = request.Content,
            };

            context.Message.Add(message);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }
    }
}
