using Api.Data;
using Api.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class BackgroundOrderUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BackgroundOrderQueue _queue;

        public BackgroundOrderUpdateService(IServiceProvider serviceProvider, BackgroundOrderQueue queue)
        {
            _serviceProvider = serviceProvider;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Background order processor started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var orderId = await _queue.DequeueAsync(stoppingToken);

                    Console.WriteLine($"Processing order: {orderId}");

                    await SimulateOrderUpdates(orderId, stoppingToken);
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Error processing order: {ex.Message}");
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

                    Console.WriteLine($"Order {orderId} status updated to: {status}");

                    // Create scope to get scoped services like DbContext
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<OnlineShopContext>();

                    var order = await dbContext.Order.FindAsync(orderId);

                    if (order == null)
                    {
                        Console.WriteLine($"Order {orderId} not found.");
                        return;
                    }

                    order.Status = status;

                    if (status == "Shipped")
                    {
                        order.DeliveryMethod = "Standard Shipping";
                        order.EstimatedDelivery = DateTime.UtcNow.AddDays(3);
                    }

                    dbContext.Entry(order).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync(ct);

                    var orderTrackingUpdate = new OrderTrackingUpdate
                    {
                        Status = status,
                        UpdatedBy = updatedBy[rnd.Next(updatedBy.Length)],
                        CreatedAt = DateTime.UtcNow
                    };

                    orderTrackingUpdate.Note = status switch
                    {
                        "Processing" => "Order received and is currently being processed by our warehouse team.",
                        "Packed" => "All items have been packed and are ready for dispatch.",
                        "Shipped" => "Order has left our warehouse and is on its way to the delivery hub.",
                        "Out for Delivery" => "Driver has your package and is en route to your address.",
                        "Delivered" => "Order delivered successfully. Thank you for shopping with us!",
                        _ => orderTrackingUpdate.Note
                    };

                    dbContext.OrderTrackingUpdate.Add(orderTrackingUpdate);
                    await dbContext.SaveChangesAsync(ct);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating order {orderId}: {ex.Message}");
                }
            }
        }
    }
}
