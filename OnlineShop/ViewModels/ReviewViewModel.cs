using Api.Models.DTOs;

namespace OnlineShopUI.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; } = string.Empty;
        public string CustomerName => $"{Customer.FirstName} {Customer.LastName}";
        public CreateCustomerDTO Customer { get; set; }
        public DateTime CreatedAt { get; set; }

        public string GetFormattedDate()
        {
            return CreatedAt.ToLocalTime().ToString("dd MMM yyyy");
        }
    }
}
