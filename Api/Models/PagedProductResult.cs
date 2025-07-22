using Api.Models.Db;

namespace Api.Models
{
    public class PagedProductResult
    {
        public List<Product> Products { get; internal set; }
        public int TotalCount { get; internal set; }
    }
}
