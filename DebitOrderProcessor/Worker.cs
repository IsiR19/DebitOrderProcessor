using DebitOrderProcessor.Services;

namespace DebitOrderProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DebitOrderProcessService _processService;
        private readonly XmlService _xmlService;

        public Worker(ILogger<Worker> logger, XmlService xmlService,DebitOrderProcessService debitOrderProcessService)
        {
            _logger = logger;
            _xmlService = xmlService;
            _processService = debitOrderProcessService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var debitOrders = _xmlService.ReadXml();
                _processService.CreateOutputFiles(debitOrders);
                _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
            }
        }
    }
}