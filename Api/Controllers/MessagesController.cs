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

            return message;
        }
    }
}
