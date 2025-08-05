namespace OnlineShopUI.Services
{
    public class BasketCountService
    {
        public event Action? OnChange;
        public int ItemCount { get; private set; }

        public void SetCount(int count)
        {
            ItemCount = count;
            NotifyStateChanged();
        }

        public void Increment(int by = 1)
        {
            ItemCount += by;
            NotifyStateChanged();
        }

        public void Decrement(int by = 1)
        {
            if (ItemCount - by < 0)
            {
                ItemCount = 0;
                NotifyStateChanged();
                return;
            }
            else
            {
                ItemCount -= by;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
