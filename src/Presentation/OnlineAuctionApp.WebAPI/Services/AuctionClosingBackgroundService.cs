using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Services;

public class AuctionClosingBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuctionClosingBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

    public AuctionClosingBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<AuctionClosingBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var auctionClosingService = scope.ServiceProvider
                    .GetRequiredService<IAuctionClosingService>();

                var closedCount = await auctionClosingService.CloseExpiredAuctionsAsync();

                if (closedCount > 0)
                {
                    _logger.LogInformation(
                        "Closed {ClosedCount} expired auction(s).",
                        closedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while closing expired auctions.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}