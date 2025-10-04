using Api.Models;
using Api.Repositories;

namespace Api.Services
{
    public class BackgroundOrderUpdateService(IServiceProvider serviceProvider, ILogger<BackgroundOrderUpdateService> logger, BackgroundOrderQueue queue) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var rnd = new Random();

            logger.LogInformation("Background order processor started.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var orderId = await queue.DequeueAsync(cancellationToken);

                    var delay = rnd.Next(1000, 5000);
                    await Task.Delay(delay, cancellationToken);

                    logger.LogInformation($"Processing order: {orderId}");

                    await SimulateOrderUpdates(orderId, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"Error processing order: {ex.Message}");
                }
            }
        }

        private async Task SimulateOrderUpdates(int orderId, CancellationToken ct)
        {
            var orderStates = new[] { "Pending", "Inventory Check", "Packed", "Shipped", "In Transit", "At local depot", "Out for Delivery", "Delivered" };
            var rnd = new Random();
            var failedOrderStack = new Stack<string>();

            try
            {
                using var scope = serviceProvider.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<OrderRepository>();

                var order = await orderRepository.GetOrder(orderId);

                if (order == null)
                {
                    logger.LogWarning($"Order {orderId} not found.");
                    return;
                }

                for (var count = 0; count < orderStates.Length; count++)
                {
                    // a random delay between 1 to 5 between each update
                    var delay = rnd.Next(1000, 5000);
                    await Task.Delay(delay, ct);

                    var status = orderStates[count];

                    logger.LogInformation($"Order {orderId} status updated to: {status}");

                    switch (status)
                    {
                        case "Pending":
                            await orderRepository.UpdateOrderStatus(orderId, status);
                            await AddNote(orderRepository, rnd, orderId, status, "Order received and is currently being processed by our warehouse team.");
                            break;

                        case "Inventory Check":
                            await orderRepository.UpdateOrderStatus(orderId, status);
                            await AddNote(orderRepository, rnd, orderId, status, "Checking inventory of ordered products by the logistics team.");
                            failedOrderStack.Push("Products add back to inventory");
                            break;

                        case "Packed":
                            await orderRepository.UpdateOrderStatus(orderId, status);
                            await AddNote(orderRepository, rnd, orderId, status, "All items have been packed and are ready to be shipped.");
                            failedOrderStack.Push("Unpacked");
                            break;

                        case "Shipped":
                            await orderRepository.UpdateOrderStatus(orderId, status, "Standard Shipping", DateTime.UtcNow.AddDays(3));
                            await AddNote(orderRepository, rnd, orderId, status, "Order has left our warehouse and is on its way to the delivery hub.");
                            failedOrderStack.Push("Back in warehouse");
                            break;

                        case "In Transit":
                            await orderRepository.UpdateOrderStatus(orderId, status);
                            await AddNote(orderRepository, rnd, orderId, status, "Order has left our warehouse and is on its way to the local depot.");
                            break;

                        case "At local depot":
                            await orderRepository.UpdateOrderStatus(orderId, status);
                            await AddNote(orderRepository, rnd, orderId, status, "Order is at the local depot ready to be delivered to the customer.");
                            failedOrderStack.Push("Return to Depot");
                            break;

                        case "Out for Delivery":
                            await orderRepository.UpdateOrderStatus(orderId, status);
                            await AddNote(orderRepository, rnd, orderId, status, "Driver has your package and is en route to your address.");
                            failedOrderStack.Push("Delivery Failed");
                            break;

                        case "Delivered":
                            if (!HasOrderFailed(order))
                            {
                                await orderRepository.UpdateOrderStatus(orderId, status);
                                await AddNote(orderRepository, rnd, orderId, status, "Order delivered successfully. Thank you for shopping with us!");
                            }
                            break;
                    }

                    logger.LogInformation($"Order {orderId} tracking update created: {status}");
                }

                if(HasOrderFailed(order))
                {
                    while(failedOrderStack.Count > 0)
                    {
                        var failedOrderUpdateStatus = failedOrderStack.Pop();

                        switch (failedOrderUpdateStatus)
                        {
                            case "Delivery Failed":
                                await AddNote(orderRepository, rnd, orderId, failedOrderUpdateStatus, "Customer not home or unavailable.");
                                break;
                            case "Return to Depot":
                                await AddNote(orderRepository, rnd, orderId, failedOrderUpdateStatus, "Package is being returned to the local depot.");
                                break;
                            case "Back in warehouse":
                                await AddNote(orderRepository, rnd, orderId, failedOrderUpdateStatus, "Items have been returned back to the warehouse waiting to be processed.");
                                break;
                            case "Unpacked":
                                await AddNote(orderRepository, rnd, orderId, failedOrderUpdateStatus, "Items have been checked and unpacked");
                                break;
                            case "Products add back to inventory":
                                await AddNote(orderRepository, rnd, orderId, failedOrderUpdateStatus, "Items have been added back to the inventory.");
                                break;
                        }
                    }

                    await orderRepository.UpdateOrderStatus(orderId, "Cancelled");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating order {orderId}: {ex.Message}");
            }
        }

        private async Task AddNote(OrderRepository orderRepository, Random random, int orderId, string status, string note)
        {
            var updatedBy = new[] { "System", "Admin", "User" };

            var orderTrackingUpdate = new OrderTrackingUpdate
            {
                OrderId = orderId,
                Status = status,
                Note = note,
                UpdatedBy = updatedBy[random.Next(updatedBy.Length)],
                CreatedAt = DateTime.UtcNow
            };

            await orderRepository.CreateTrackingUpdate(orderTrackingUpdate);
        }

        private bool HasOrderFailed(Order order)
        {
            return order.Id % 2 == 0;
        }
    }
}