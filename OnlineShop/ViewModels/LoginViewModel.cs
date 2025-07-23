using System.ComponentModel.DataAnnotations;

namespace OnlineShopUI.ViewModels
{
    public class LoginViewModel
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}
