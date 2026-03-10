using Api.Models;
using Api.Repositories;

namespace Api.Services
{
    public class BackgroundOrderUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundOrderUpdateService> _logger;
        private readonly BackgroundOrderQueue _queue;

        public BackgroundOrderUpdateService(IServiceProvider serviceProvider, ILogger<BackgroundOrderUpdateService> logger, BackgroundOrderQueue queue)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var orderId = _queue.Dequeue();

                if (orderId != null)
                {
                    _logger.LogInformation($"Processing order: {orderId}");

                    await ProcessOrder(orderId.Value, cancellationToken);
                }

                await Task.Delay(2000, cancellationToken);
            }
        }

        private async Task ProcessOrder(int orderId, CancellationToken ct)
        {
            List<StatusUpdate> _successStateUpdates = new List<StatusUpdate>
            {
                new StatusUpdate { State = "Inventory check", Note = "Checking stock levels." },
                new StatusUpdate { State = "Shipped", Note = "Order is on its way to the hub." },
                new StatusUpdate { State = "At local post office", Note = "Order is at the local post office." },
                new StatusUpdate { State = "Delivered", Note = "Order has been delivered!" }
            };

            List<StatusUpdate> _failedStateUpdates = new List<StatusUpdate>
            {
                new StatusUpdate { State = "Add to inventory", Note = "Items added back to inventory." },
                new StatusUpdate { State = "Back in warehouse", Note = "Items have been returned" },
                new StatusUpdate { State = "Return to post office", Note = "Package returned to the local post office." },
                new StatusUpdate { State = "Delivery failed", Note = "Customer not home." },
            };


            // using a stack to keep track of steps if something goes wrong
            var failedOrderStack = new Stack<StatusUpdate>();

            using var scope = _serviceProvider.CreateScope();
            var orderRepository = scope.ServiceProvider.GetRequiredService<OrderRepository>();

            var order = await orderRepository.GetOrder(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order {orderId} not found.");
                return;
            }

            for (var i = 0; i < _successStateUpdates.Count; i++)
            {
                await Task.Delay(3000, ct);

                failedOrderStack.Push(_failedStateUpdates[i]);

                if (_successStateUpdates[i].State == "Delivered" && order.Id % 2 == 0)
                    break;

                if (_successStateUpdates[i].State == "Shipped")
                {
                    await orderRepository.UpdateOrderStatus(orderId, _successStateUpdates[i].State, "Standard Shipping", DateTime.UtcNow.AddDays(3));
                }
                else
                {
                    await orderRepository.UpdateOrderStatus(orderId, _successStateUpdates[i].State);
                }

                await AddNote(orderRepository, orderId, _successStateUpdates[i].State, _successStateUpdates[i].Note);

                _logger.LogInformation($"Order {orderId} tracking update created: {_successStateUpdates[i].State}");
            }

            // simulating failed delivery on some orders
            if (order.Id % 2 == 0)
            {
                while (failedOrderStack.Count > 0)
                {
                    var failedStatusUpdate = failedOrderStack.Pop();

                    await Task.Delay(2000, ct);

                    await AddNote(orderRepository, orderId, failedStatusUpdate.State, failedStatusUpdate.Note);
                }

                await orderRepository.UpdateOrderStatus(orderId, "Cancelled");
            }
        }

        private async Task AddNote(OrderRepository orderRepository, int orderId, string status, string note)
        {
            await orderRepository.CreateTrackingUpdate(new OrderTrackingUpdate
            {
                OrderId = orderId,
                Status = status,
                Note = note,
                UpdatedBy = "System",
                CreatedAt = DateTime.UtcNow
            });
        }
    }

    public class StatusUpdate
    {
        public string State { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}