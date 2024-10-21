using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductGroupingApi.Services;

public class HostedProductGroupingService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider; 
    private readonly ILogger<HostedProductGroupingService> _logger;
    private Timer _timer;

    public HostedProductGroupingService(IServiceProvider serviceProvider, ILogger<HostedProductGroupingService> logger)
    {
        _serviceProvider = serviceProvider; 
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product Grouping Task started.");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        using (var scope = _serviceProvider.CreateScope()) 
        {
            var groupingService = scope.ServiceProvider.GetRequiredService<ProductGroupingService>(); 
            try
            {
                _logger.LogInformation("Grouping products...");
                groupingService.GroupProducts(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while grouping products.");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product Grouping Task stopped.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
