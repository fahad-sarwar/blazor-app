namespace OnlineShopUI.ViewModels
{
    public class BasketViewModel
    {
        public int Id { get; set; }
        public string? AnonymousId { get; set; }
        public List<BasketItemViewModel> Items { get; set; } = new();

        public double Vat()
        {
            return Total() * GetVatRate();
        }

        public string FormattedVat()
        {
            return Vat().ToString("C");
        }

        public double Total()
        {
            return Items.Sum(item => item.LineTotal());
        }

        public string FormattedTotal()
        {
            return Total().ToString("C");
        }

        private double GetVatRate()
        {
            return Items.FirstOrDefault()?.VATRate ?? 0.2;
        }
    }
}
