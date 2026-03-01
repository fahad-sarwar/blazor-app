using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(CategoryRepository categoryRepository, ILogger<CategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetCategories();
                return Ok(categories);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There was an error getting all categories.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategory(id);

                return category == null
                    ? NotFound()
                    : Ok(category);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There was an error getting a category with id {CategoryId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
