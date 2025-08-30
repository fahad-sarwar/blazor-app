using Api.Data;
using Api.Models;
using Api.Models.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(OnlineShopContext context, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            return await context.Customer.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, UpdateCustomerRequest request)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return NotFound("Customer not found.");

            if (id != customer.Id)
            {
                return BadRequest();
            }

            var user = await userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return NotFound("User not found.");
            }

            if(user.Email != customer.Email)
            {
                return BadRequest("User email does not match customer email.");
            }

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
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            context.Customer.Add(customer);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            context.Customer.Remove(customer);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return context.Customer.Any(e => e.Id == id);
        }
    }
}
