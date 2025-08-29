using Api.Models.Db;

namespace Api.Models
{
    public class PagedReviewResult
    {
        public List<Review> Reviews { get; internal set; }
        public int TotalCount { get; internal set; }
    }
}
