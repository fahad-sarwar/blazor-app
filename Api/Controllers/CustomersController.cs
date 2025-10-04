using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(CustomerRepository customerRepository, UserRepository userRepository, 
        ILogger<CustomersController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("The system was unable to find the customer.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error retrieving customer details.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDTO request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("The system was unable to find the Customer.  Please provide the correct details.");
                }

                if (id != customer.Id)
                {
                    return BadRequest("The entered customer details do not match the system records.  Please provide the correct details.");
                }

                var user = await userRepository.GetUserByUsername(email);

                if (user == null)
                {
                    return NotFound("A user matching the email was not found.  Please provide the correct customer details.");
                }

                if (user.Username != customer.Email)
                {
                    return BadRequest("The user details do not match the customer email.  Please provide the correct customer details.");
                }

                customer.FirstName = request.FirstName;
                customer.LastName = request.LastName;
                customer.PhoneNumber = request.PhoneNumber;

                var createShippingAddress = customer.ShippingAddress == null;
                var createBillingAddress = customer.BillingAddress == null;

                if (createShippingAddress)
                {
                    var shippingAddress = new Address
                    {
                        AddressLineOne = request.ShippingAddressLineOne,
                        AddressLineTwo = request.ShippingAddressLineTwo,
                        Town = request.ShippingTown,
                        County = request.ShippingCounty,
                        PostCode = request.ShippingPostCode,
                        Country = request.ShippingCountry
                    };

                    shippingAddress = await customerRepository.CreateAddress(shippingAddress);
                    customer.ShippingAddress = shippingAddress;
                }
                else
                {
                    customer.ShippingAddress.AddressLineOne = request.ShippingAddressLineOne;
                    customer.ShippingAddress.AddressLineTwo = request.ShippingAddressLineTwo;
                    customer.ShippingAddress.Town = request.ShippingTown;
                    customer.ShippingAddress.County = request.ShippingCounty;
                    customer.ShippingAddress.PostCode = request.ShippingPostCode;
                    customer.ShippingAddress.Country = request.ShippingCountry;
                    await customerRepository.UpdateAddress(customer.ShippingAddress);
                }           

                if (createBillingAddress)
                {
                    var billingAddress = new Address
                    {
                        AddressLineOne = request.BillingAddressLineOne,
                        AddressLineTwo = request.BillingAddressLineTwo,
                        Town = request.BillingTown,
                        County = request.BillingCounty,
                        PostCode = request.BillingPostCode,
                        Country = request.BillingCountry
                    };

                    billingAddress = await customerRepository.CreateAddress(billingAddress);
                    customer.BillingAddress = billingAddress;
                }
                else
                {
                    customer.BillingAddress.AddressLineOne = request.BillingAddressLineOne;
                    customer.BillingAddress.AddressLineTwo = request.BillingAddressLineTwo;
                    customer.BillingAddress.Town = request.BillingTown;
                    customer.BillingAddress.County = request.BillingCounty;
                    customer.BillingAddress.PostCode = request.BillingPostCode;
                    customer.BillingAddress.Country = request.BillingCountry;
                    await customerRepository.UpdateAddress(customer.BillingAddress);
                }

                await customerRepository.UpdateCustomer(customer);

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "There was an error updating the customers account with id {CustomerId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
