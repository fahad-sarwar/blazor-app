using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, [FromQuery] bool? forSale, [FromQuery] string? searchTerm,
            [FromQuery] string? sort = "name-asc", [FromQuery] int page = 1, [FromQuery] int pageSize = 9)
        {
            try
            {
                if (!categoryId.HasValue && !forSale.HasValue && string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("At least one filter must be provided: category, forSale, or searchTerm.");
                }

                if (page < 1 || pageSize < 1)
                {
                    return BadRequest("Page and pageSize must be greater than 0.");
                }

                if (categoryId.HasValue && categoryId < 0)
                {
                    return BadRequest("Category ID must be a positive integer.");
                }

                var (products, totalCount) = await productRepository.GetProducts(categoryId, forSale, searchTerm, sort, page, pageSize);

                return Ok(
                    new
                    {
                        Products = products,
                        TotalCount = totalCount
                    }
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving products");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }            
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await productRepository.GetProduct(id);

                return product == null
                    ? NotFound()
                    : Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving product with id {ProductId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
