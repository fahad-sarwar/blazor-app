using Api.Data;
using Api.Models;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly OnlineShopContext _context;

        public ProductsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: api/Products
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

            var query = _context.Product.AsQueryable();

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

            // Total before paging
            var totalCount = await query.CountAsync();

            // Apply sorting
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


        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
