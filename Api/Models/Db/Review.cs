namespace Api.Models.Db
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; } = string.Empty;
        public Product Product { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
