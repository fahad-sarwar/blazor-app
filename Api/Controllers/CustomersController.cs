using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerRepository customerRepository, IAddressRepository addressRepository, 
        IUserRepository userRepository, ILogger<CustomersController> logger) : ControllerBase
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
                    return NotFound("Customer not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving customer");
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
                    return NotFound("Customer not found.");
                }

                if (id != customer.Id)
                {
                    return BadRequest("Invalid customer identified.");
                }

                var user = await userRepository.GetUserByUserName(email);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (user.UserName != customer.Email)
                {
                    return BadRequest("User email does not match customer email.");
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

                    shippingAddress = await addressRepository.CreateAddress(shippingAddress);
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
                    await addressRepository.UpdateAddress(customer.ShippingAddress);
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

                    billingAddress = await addressRepository.CreateAddress(billingAddress);
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
                    await addressRepository.UpdateAddress(customer.BillingAddress);
                }

                await customerRepository.UpdateCustomer(customer);

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error updating customer with id {CustomerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
