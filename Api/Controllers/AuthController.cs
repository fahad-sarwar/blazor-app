using System.Security.Claims;
using Api.Data;
using Api.Models;
using Api.Models.Db;
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
        OnlineShopContext context)
        : ControllerBase
    {
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
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
                Console.WriteLine(e);
                return BadRequest("Error whilst registering user.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
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

        [HttpPost("refresh-claims")]
        public async Task<IActionResult> RefreshClaims()
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
