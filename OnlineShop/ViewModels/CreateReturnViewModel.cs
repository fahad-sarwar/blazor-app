namespace OnlineShopUI.ViewModels
{
    public class CreateReturnViewModel
    {
        public int OrderId { get; set; }
        public string Comments { get; set; }
        public List<CreateReturnItemViewModel> Items { get; set; } = [];
    }
}
