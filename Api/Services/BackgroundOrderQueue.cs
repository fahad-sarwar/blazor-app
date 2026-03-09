namespace Api.Services
{
    public class BackgroundOrderQueue
    {
        private readonly Queue<int> _orders = new();

        public void Enqueue(int orderId)
        {
            _orders.Enqueue(orderId);
        }

        public int? Dequeue()
        {
            if (_orders.Count == 0)
            {
                return null;
            }
            
            return _orders.Dequeue();
        }
    }
}
