using System.Security.Claims;
using Api.Data;
using Api.Models;
using Api.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        OnlineShopContext context, 
        ILogger<AuthController> logger)
        : ControllerBase
    {
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                if (User.Identity?.IsAuthenticated != true)
                    return Unauthorized();

                var user = await userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var customer = await context.Customer
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                return Ok(new
                {
                    Email = user.Email,
                    Name = user.FirstName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserId = user.Id,
                    CustomerId = customer?.Id,
                    PhoneNumber = customer?.PhoneNumber
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
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserId = user.Id,
                    User = user
                };

                context.Customer.Add(customer);
                await context.SaveChangesAsync();

                await signInManager.SignInAsync(user, isPersistent: false);
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
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (!result.Succeeded)
                    return Unauthorized();

                var user = await userManager.FindByEmailAsync(model.Email);
                var customer = await context.Customer
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim("UserId", user.Id),
                    new Claim("CustomerId", customer.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

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
                    return Unauthorized();

                var user = await userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var customer = await context.Customer
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim("UserId", user.Id),
                    new Claim("CustomerId", customer?.Id.ToString() ?? string.Empty)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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
                await signInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error logging out user");
                return StatusCode(500, "Error logging out user");
            }
        }
    }
}
