namespace Api.Models
{
    public class CreateReturnRequest
    {
        public int OrderId { get; set; }
        public string Comments { get; set; }
        public List<CreateReturnItemRequest> Items { get; set; } = [];
    }
}
