using System.Collections.Concurrent;

namespace Api.Services
{
    public class BackgroundOrderQueue
    {
        private readonly ConcurrentQueue<int> _orders = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void Enqueue(int orderId)
        {
            _orders.Enqueue(orderId);
            _signal.Release();
        }

        public async Task<int> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _orders.TryDequeue(out var orderId);
            return orderId;
        }
    }
}
