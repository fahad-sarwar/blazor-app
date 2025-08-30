using System.Security.Claims;
using Api.Data;
using Api.Models;
using Api.Models.Db;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnsController(OnlineShopContext context, BackgroundOrderQueue queue) : ControllerBase
    {
        // GET: api/Returns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Return>> GetReturn(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null)
                return NotFound("Customer not found.");

            var customerReturn = await context.Return
                .Include(r => r.Customer)
                .Include(r => r.Order)
                .Include(r => r.ReturnItems)
                    .ThenInclude(ri => ri.Product)
                .Include(r => r.ReturnItems)
                    .ThenInclude(ri => ri.OrderItem)
                        .ThenInclude(oi => oi.Product)
                .SingleOrDefaultAsync(o => o.Id == id && o.Customer.Id == customer.Id);

            if (customerReturn == null)
            {
                return NotFound();
            }

            return customerReturn;
        }

        // POST: api/Returns
        [HttpPost]
        public async Task<ActionResult<Return>> PostReturn(CreateReturnRequest createReturnRequest)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);
            
            if (customer == null)
                return NotFound("Customer not found.");

            var order = await context.Order
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == createReturnRequest.OrderId && o.Customer.Id == customer.Id);
            
            if (order == null)
                return BadRequest("Invalid order ID.");

            foreach(var returnItem in createReturnRequest.Items)
            {
                var validItem = await context.OrderItem
                    .Include(oi => oi.Product)
                    .FirstOrDefaultAsync(oi => oi.Id == returnItem.OrderItemId && oi.OrderId == createReturnRequest.OrderId);

                if (validItem == null)
                    return BadRequest($"Invalid return item ID: {returnItem.OrderItemId}");
            }

            var customerReturn = new Return
            {
                Order = order,
                Customer = customer,
                Status = "Requested",
                Comments = createReturnRequest.Comments,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Return.Add(customerReturn);
            await context.SaveChangesAsync();

            customerReturn.ReturnNumber = $"RTN{customerReturn.Id:D7}";
            await context.SaveChangesAsync(); // Save the ReturnNumber

            // Create return items
            var returnItems = new List<ReturnItem>();

            foreach (var orderItem in order.OrderItems)
            {
                if(createReturnRequest.Items.Any(ri => ri.OrderItemId == orderItem.Id))
                {
                    var customerReturnItem = new ReturnItem
                    {
                        ReturnId = customerReturn.Id,
                        Product = orderItem.Product,
                        OrderItem = orderItem,
                        Quantity = orderItem.Quantity,
                        ReturnReason = createReturnRequest.Items.Single(ri => ri.OrderItemId == orderItem.Id).Reason,
                        UnitPrice = orderItem.UnitPrice,
                        TotalPrice = orderItem.TotalPrice,
                        VATRate = orderItem.VATRate,
                        CreatedAt = DateTime.UtcNow
                    };

                    returnItems.Add(customerReturnItem);
                }
            }

            context.ReturnItem.AddRange(returnItems);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetReturn", new { id = customerReturn.Id }, customerReturn);
        }
    }
}
