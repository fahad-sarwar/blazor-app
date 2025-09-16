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
    public class AuthController(IUserRepository userRepository, ICustomerRepository customerRepository, ILogger<AuthController> logger) : ControllerBase
    {
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                logger.LogInformation("GetCurrentUser called. IsAuthenticated: {IsAuthenticated}, Identity: {Identity}", 
                    User.Identity?.IsAuthenticated, User.Identity?.Name);
                
                if (User.Identity?.IsAuthenticated != true)
                {
                    logger.LogWarning("User is not authenticated");
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
                    Username = user.UserName,
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
                logger.LogError(e, "Error retrieving current user");
                return StatusCode(500, "Error retrieving current user");
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
                    return BadRequest("Username already exists");
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
                logger.LogError(e, "Error registering user");
                return StatusCode(500, "Error registering user");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var user = await userRepository.GetUserByUserName(model.Email);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                var isValidPassword = await userRepository.ValidatePassword(model.Email, model.Password);

                if (!isValidPassword)
                {
                    return Unauthorized("Invalid username or password");
                }

                var customer = await customerRepository.GetCustomerByUserId(user.Id.ToString());

                if (customer == null)
                {
                    logger.LogWarning("User {Username} found but no corresponding customer record", user.UserName);
                    return Unauthorized("Account configuration error");
                }

                await SignInUserAsync(user, customer);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error logging in user");
                return StatusCode(500, "Error logging in user");
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
                logger.LogError(e, "Error refreshing claims");
                return StatusCode(500, "Error refreshing claims");
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
                logger.LogError(e, "Error logging out user");
                return StatusCode(500, "Error logging out user");
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

            logger.LogInformation("Signing in user {UserId} with {ClaimCount} claims: {Claims}", 
                user.Id, claims.Count, string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}")));
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            logger.LogInformation("User {UserId} signed in successfully", user.Id);
        }
    }
}
