using Api.Models;
using Api.Repositories;

namespace Api.Services
{
    public class BackgroundOrderUpdateService(IServiceProvider serviceProvider, ILogger<BackgroundOrderUpdateService> logger, BackgroundOrderQueue queue) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Background order processor started.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var orderId = await queue.DequeueAsync(cancellationToken);

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
            var statuses = new[] { "Processing", "Packed", "Shipped", "Out for Delivery", "Delivered" };
            var updatedBy = new[] { "System", "Admin", "User" };
            var rnd = new Random();

            foreach (var status in statuses)
            {
                try
                {
                    var delay = rnd.Next(1000, 5000); // 1 to 5 seconds
                    await Task.Delay(delay, ct);

                    logger.LogInformation($"Order {orderId} status updated to: {status}");

                    using var scope = serviceProvider.CreateScope();
                    var orderRepository = scope.ServiceProvider.GetRequiredService<OrderRepository>();
                    var trackingUpdateRepository = scope.ServiceProvider.GetRequiredService<OrderTrackingUpdateRepository>();

                    var order = await orderRepository.GetOrder(orderId);

                    if (order == null)
                    {
                        logger.LogWarning($"Order {orderId} not found.");
                        return;
                    }

                    if (status == "Shipped")
                    {
                        await orderRepository.UpdateOrderStatus(orderId, status, "Standard Shipping", DateTime.UtcNow.AddDays(3));
                    }
                    else
                    {
                        await orderRepository.UpdateOrderStatus(orderId, status);
                    }

                    var orderTrackingUpdate = new OrderTrackingUpdate
                    {
                        OrderId = orderId,
                        Status = status,
                        UpdatedBy = updatedBy[rnd.Next(updatedBy.Length)],
                        CreatedAt = DateTime.UtcNow
                    };

                    var note = string.Empty;

                    switch(status)
                    {
                        case "Processing":
                            note = "Order received and is currently being processed by our warehouse team.";
                            break;
                        case "Packed":
                            note = "All items have been packed and are ready for dispatch.";
                            break;
                        case "Shipped":
                            note = "Order has left our warehouse and is on its way to the delivery hub.";
                            break;
                        case "Out for Delivery":
                            note = "Driver has your package and is en route to your address.";
                            break;
                        case "Delivered":
                            note = "Order delivered successfully. Thank you for shopping with us!";
                            break;
                    };
                    
                    orderTrackingUpdate.Note = note;

                    await trackingUpdateRepository.CreateTrackingUpdate(orderTrackingUpdate);

                    logger.LogInformation($"Order {orderId} tracking update created: {status}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error updating order {orderId}: {ex.Message}");
                }
            }
        }
    }
}
