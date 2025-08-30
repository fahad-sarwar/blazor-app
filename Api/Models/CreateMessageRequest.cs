namespace Api.Models
{
    public class CreateMessageRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
