namespace Api.Services
{
    public class BackgroundReturnUpdateService(IServiceProvider serviceProvider, BackgroundOrderQueue queue)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Background order processor started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("Processing request");

                    await Task.Delay(10000, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing order: {ex.Message}");
                }
            }
        }
    }
}
