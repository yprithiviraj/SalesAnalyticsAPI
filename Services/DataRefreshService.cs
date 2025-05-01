namespace SalesAnalyticsApi.Services
{
    public class DataRefreshService : BackgroundService
    {
        private readonly ILogger<DataRefreshService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _refreshInterval = TimeSpan.FromHours(24); //Refresh every 24 hours

        public DataRefreshService(ILogger<DataRefreshService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Data refresh background service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Data refresh started at: {time}", DateTimeOffset.Now);

                    await RefreshDataAsync();

                    _logger.LogInformation("Data refresh completed at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while refreshing data.");
                }

                await Task.Delay(_refreshInterval, stoppingToken);
            }

            _logger.LogInformation("Data refresh background service is stopping.");
        }

        private async Task RefreshDataAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var csvService = scope.ServiceProvider.GetRequiredService<ICsvImportService>();

            //Here we just make use of existing import csv method
            //This is done under assumption that we have csv file at a designated location (s3 or app server etc.,)
            //that is updated periodically so we just read it periodically to keep our database updated
            await csvService.ImportCsvAsync();
        }
    }
}

