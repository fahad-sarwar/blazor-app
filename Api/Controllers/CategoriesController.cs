using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(OnlineShopContext context, ILogger<CategoriesController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await context.Category.ToListAsync();
                return Ok(categories);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error retrieving categories");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await context.Category.FindAsync(id);

                return category == null
                    ? NotFound()
                    : Ok(category);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error retrieving category with id {CategoryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
