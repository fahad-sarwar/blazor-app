using Api.Models.Db;

namespace Api.Models
{
    public class PagedOrderResult
    {
        public List<Order> Orders { get; internal set; }
        public int TotalCount { get; internal set; }
    }
}
