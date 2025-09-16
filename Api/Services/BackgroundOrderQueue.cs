using System.Collections.Concurrent;

namespace Api.Services
{
    public class BackgroundOrderQueue
    {
        private readonly ConcurrentQueue<int> _orders = new();

        public void Enqueue(int orderId)
        {
            _orders.Enqueue(orderId);
        }

        public async Task<int> DequeueAsync(CancellationToken cancellationToken)
        {
            _orders.TryDequeue(out var orderId);
            return orderId;
        }
    }
}
