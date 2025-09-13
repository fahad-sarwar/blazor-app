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
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer
                    .Include(c => c.ShippingAddress)
                    .Include(c => c.BillingAddress)
                    .FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving customer");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, UpdateCustomerDTO request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer
                    .Include(c => c.ShippingAddress)
                    .Include(c => c.BillingAddress)
                    .FirstOrDefaultAsync(c => c.Email == email);

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

                var shippingAddress = customer.ShippingAddress;
                var billingAddress = customer.BillingAddress;

                var createShippingAddress = customer.ShippingAddress == null;
                var createBillingAddress = customer.BillingAddress == null;

                if (createShippingAddress)
                {
                    shippingAddress = new Address
                    {
                        AddressLineOne = request.ShippingAddressLineOne,
                        AddressLineTwo = request.ShippingAddressLineTwo,
                        Town = request.ShippingTown,
                        County = request.ShippingCounty,
                        PostCode = request.ShippingPostCode,
                        Country = request.ShippingCountry
                    };
                    context.Address.Add(shippingAddress);
                    customer.ShippingAddress = shippingAddress;
                }

                if (createBillingAddress)
                {
                    billingAddress = new Address
                    {
                        AddressLineOne = request.BillingAddressLineOne,
                        AddressLineTwo = request.BillingAddressLineTwo,
                        Town = request.BillingTown,
                        County = request.BillingCounty,
                        PostCode = request.BillingPostCode,
                        Country = request.BillingCountry
                    };
                    context.Address.Add(billingAddress);
                    customer.BillingAddress = billingAddress;
                }

                if (!createShippingAddress)
                {
                    shippingAddress.AddressLineOne = request.ShippingAddressLineOne;
                    shippingAddress.AddressLineTwo = request.ShippingAddressLineTwo;
                    shippingAddress.Town = request.ShippingTown;
                    shippingAddress.County = request.ShippingCounty;
                    shippingAddress.PostCode = request.ShippingPostCode;
                    shippingAddress.Country = request.ShippingCountry;
                }

                if(!createBillingAddress)
                {
                    billingAddress.AddressLineOne = request.BillingAddressLineOne;
                    billingAddress.AddressLineTwo = request.BillingAddressLineTwo;
                    billingAddress.Town = request.BillingTown;
                    billingAddress.County = request.BillingCounty;
                    billingAddress.PostCode = request.BillingPostCode;
                    billingAddress.Country = request.BillingCountry;
                }

                context.Entry(customer).State = EntityState.Modified;
                context.Entry(user).State = EntityState.Modified;
                
                context.Entry(shippingAddress).State = createShippingAddress 
                    ? EntityState.Added 
                    : EntityState.Modified;

                context.Entry(billingAddress).State = createBillingAddress
                    ? EntityState.Added
                    : EntityState.Modified;

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
