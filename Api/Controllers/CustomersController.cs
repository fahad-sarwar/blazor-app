using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;
        private readonly UserRepository _userRepository;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(CustomerRepository customerRepository, UserRepository userRepository, 
            ILogger<CustomersController> logger)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

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

                var customer = await _customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error retrieving customer details.");
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

                var customer = await _customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                if (id != customer.Id)
                {
                    return BadRequest("Customer doesn't match.");
                }

                var user = await _userRepository.GetUserByUsername(email);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (user.Username != customer.Email)
                {
                    return BadRequest("User doesn't match customer.");
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

                    shippingAddress = await _customerRepository.CreateAddress(shippingAddress);
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
                    await _customerRepository.UpdateAddress(customer.ShippingAddress);
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

                    billingAddress = await _customerRepository.CreateAddress(billingAddress);
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
                    await _customerRepository.UpdateAddress(customer.BillingAddress);
                }

                await _customerRepository.UpdateCustomer(customer);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There was an error updating the customers account with id {CustomerId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
