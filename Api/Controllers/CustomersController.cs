using Api.Data;
using Api.Models;
using Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(OnlineShopContext context, UserManager<ApplicationUser> userManager, ILogger<CustomersController> logger) : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, UpdateCustomerDTO request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                if (id != customer.Id)
                    return BadRequest();

                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                    return NotFound("User not found.");

                if (user.Email != customer.Email)
                    return BadRequest("User email does not match customer email.");

                customer.FirstName = request.FirstName;
                customer.LastName = request.LastName;
                customer.PhoneNumber = request.PhoneNumber;

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                context.Entry(customer).State = EntityState.Modified;
                context.Entry(user).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                        return NotFound();

                    throw;
                }

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error updating customer with id {CustomerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool CustomerExists(int id)
        {
            return context.Customer.Any(e => e.Id == id);
        }
    }
}
