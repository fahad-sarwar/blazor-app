using Api.Data;
using Api.Models;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(OnlineShopContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedProductResult>> GetProduct(
            [FromQuery] int? categoryId,
            [FromQuery] bool? forSale,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sort = "name-asc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 9)
        {
            if (!categoryId.HasValue && !forSale.HasValue && string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("At least one filter must be provided: category, forSale, or searchTerm.");

            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than 0.");

            if(categoryId.HasValue && categoryId < 0)
                return BadRequest("Category ID must be a positive integer.");

            var query = context.Product.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.Category.Id == categoryId.Value);
            }

            if (forSale.HasValue)
            {
                query = query.Where(p => p.ForSale == forSale.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalised = searchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(normalised) ||
                                         p.Description.ToLower().Contains(normalised));
            }

            var totalCount = await query.CountAsync();

            query = sort switch
            {
                "name-asc" => query.OrderBy(p => p.Name),
                "name-desc" => query.OrderByDescending(p => p.Name),
                "price-asc" => query.OrderBy(p => p.Price),
                "price-desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            var paged = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedProductResult
            {
                Products = paged,
                TotalCount = totalCount
            };
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
    }
}
