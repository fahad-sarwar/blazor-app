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
            var orderStates = new[] { "Pending", "Inventory check", "Packed", "Shipped", "In transit", "At local post office", "Out for delivery", "Delivered" };

            var stateNotes = new Dictionary<string, string>
            {
                { "Pending", "Order received." },
                { "Inventory check", "Checking stock levels." },
                { "Packed", "Items packed and ready to be shipped." },
                { "Shipped", "Order is on its way to the hub." },
                { "In transit", "Order on its way to the local post office." },
                { "At local post office", "Order is at the local post office." },
                { "Out for delivery", "Package is on its way to the customer." },
                { "Delivered", "Order has been delivered!" }
            };

            var failedStateNotes = new Dictionary<string, string>
            {
                { "Delivery failed", "Customer not home or unavailable." },
                { "Return to post office", "Package is being returned to the local post office." },
                { "Back in warehouse", "Items have been returned back to the warehouse waiting to be processed." },
                { "Unpacked", "Items have been checked and unpacked" },
                { "Add to inventory", "Items have been added back to the inventory." },
            };

            var failedOrderStack = new Stack<string>();

            using var scope = _serviceProvider.CreateScope();
            var orderRepository = scope.ServiceProvider.GetRequiredService<OrderRepository>();

            var order = await orderRepository.GetOrder(orderId);

            if (order == null)
            {
                _logger.LogWarning($"Order {orderId} not found.");
                return;
            }

            for (var i = 0; i < orderStates.Length; i++)
            {
                await Task.Delay(3000, ct);

                string? deliveryMethod = null;
                DateTime? estimatedDelivery = null;
                var status = orderStates[i];

                switch (status)
                {
                    case "Inventory check":
                        failedOrderStack.Push("Add to inventory");
                        break;

                    case "Packed":
                        failedOrderStack.Push("Unpacked");
                        break;

                    case "Shipped":
                        deliveryMethod = "Standard Shipping";
                        estimatedDelivery = DateTime.UtcNow.AddDays(3);
                        failedOrderStack.Push("Back in warehouse");
                        break;

                    case "At local post office":
                        failedOrderStack.Push("Return to post office");
                        break;

                    case "Out for delivery":
                        failedOrderStack.Push("Delivery failed");
                        break;
                }

                if (status == "Delivered" && order.Id % 2 == 0)
                    break;

                await orderRepository.UpdateOrderStatus(orderId, status, deliveryMethod, estimatedDelivery);
                await AddNote(orderRepository, orderId, status, stateNotes[status]);

                _logger.LogInformation($"Order {orderId} tracking update created: {status}");
            }

            if (order.Id % 2 == 0)
            {
                while (failedOrderStack.Count > 0)
                {
                    var status = failedOrderStack.Pop();

                    await Task.Delay(2000, ct);

                    await AddNote(orderRepository, orderId, status, failedStateNotes[status]);
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
}