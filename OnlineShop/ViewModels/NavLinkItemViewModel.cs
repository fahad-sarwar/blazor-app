namespace OnlineShopUI.ViewModels
{
    public class NavLinkItemViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public bool IsExternal { get; set; }
    }
}
