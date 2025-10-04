using System.Security.Claims;
using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserRepository userRepository, CustomerRepository customerRepository, ILogger<AuthController> logger) : ControllerBase
    {
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                logger.LogInformation("Getting the current user to see if they are authenticated. IsAuthenticated: {IsAuthenticated}, Identity: {Identity}", 
                    User.Identity?.IsAuthenticated, User.Identity?.Name);
                
                if (User.Identity?.IsAuthenticated != true)
                {
                    logger.LogWarning("The user is not currently logged in.");
                    return Unauthorized();
                }

                var userId = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
                {
                    return Unauthorized();
                }

                var user = await userRepository.GetUserById(userIdInt);

                if (user == null)
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByUserId(userId);

                return Ok(new
                {
                    Username = user.Username,
                    Email = customer.Email,
                    Name = customer.FirstName,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    UserId = user.Id.ToString(),
                    CustomerId = customer?.Id,
                    PhoneNumber = customer?.PhoneNumber,
                    IsAdmin = user.IsAdmin
                });
            }
            catch (Exception e)
            {
                logger.LogError(e, "There was an error getting current user details.  Exception message: '{Message}'", e.Message);
                return StatusCode(500, "Error getting the current user.");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                var existingUser = await userRepository.UserExists(model.Email);

                if (existingUser)
                {
                    return BadRequest("The current username already exists");
                }

                var user = await userRepository.CreateUser(model.Email, model.Password, false);

                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserId = user.Id.ToString(),
                    CreatedAt = DateTime.UtcNow
                };

                await customerRepository.CreateCustomer(customer);

                await SignInUserAsync(user, customer);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "There was an error registering user.  Exception message: '{Message}'", e.Message);
                return StatusCode(500, "Error registering the user.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var user = await userRepository.GetUserByUsername(model.Email);

                if (user == null)
                {
                    return Unauthorized("The user doesn't exist.");
                }

                var isValidPassword = await userRepository.ValidatePassword(model.Email, model.Password);

                if (!isValidPassword)
                {
                    return Unauthorized("The entered password does not match the current users password.");
                }

                var customer = await customerRepository.GetCustomerByUserId(user.Id.ToString());

                if (customer == null)
                {
                    logger.LogWarning("User {Username} found but no there was no corresponding customer record.", user.Username);
                    return Unauthorized("Unable to find the customers account.");
                }

                await SignInUserAsync(user, customer);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "There was an error whilst logging in the user.");
                return StatusCode(500, "Error logging in the user.");
            }
        }

        [HttpPost("refresh-claims")]
        public async Task<IActionResult> RefreshClaims()
        {
            try
            {
                if (User.Identity?.IsAuthenticated != true)
                {
                    return Unauthorized();
                }

                var userId = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
                {
                    return Unauthorized();
                }

                var user = await userRepository.GetUserById(userIdInt);

                if (user == null)
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByUserId(user.Id.ToString());

                if (customer != null)
                {
                    await SignInUserAsync(user, customer);
                }

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occured whilst refreshing the users claims.");
                return StatusCode(500, "Error refreshing the users claims.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "There was an error whilst logging out user.");
                return StatusCode(500, "Error whilst logging out the user.");
            }
        }

        private async Task SignInUserAsync(User user, Customer customer)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.FirstName),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.GivenName, customer.FirstName),
                new Claim(ClaimTypes.Surname, customer.LastName),
                new Claim("UserId", user.Id.ToString()),
                new Claim("CustomerId", customer.Id.ToString()),
                new Claim("IsAdmin", user.IsAdmin.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            logger.LogInformation("Signing in the user {UserId} with {ClaimCount} active claims: {Claims}", 
                user.Id, claims.Count, string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}")));
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            logger.LogInformation("The user with id ({UserId}) has signed in successfully.", user.Id);
        }
    }
}
