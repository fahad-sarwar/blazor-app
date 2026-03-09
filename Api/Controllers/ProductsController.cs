using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, [FromQuery] bool? forSale, [FromQuery] string? searchTerm,
            [FromQuery] string? sort = "name-asc", [FromQuery] int page = 1, [FromQuery] int pageSize = 9)
        {
            if (!categoryId.HasValue && !forSale.HasValue && string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("At least one of the following filters must be provided: category, forSale, or searchTerm.");
            }

            if (page < 1 || pageSize < 1)
            {
                return BadRequest("The page and pageSize fields must be greater than 0.");
            }

            if (categoryId.HasValue && categoryId < 0)
            {
                return BadRequest("The category id must be a positive integer.  Please enter the correct details.");
            }

            var (products, totalCount) = await _productRepository.GetProducts(categoryId, forSale, searchTerm, sort, page, pageSize);

            return Ok(
                new
                {
                    Products = products,
                    TotalCount = totalCount
                }
            );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetProduct(id);

            return product == null
                ? NotFound()
                : Ok(product);
        }
    }
}
