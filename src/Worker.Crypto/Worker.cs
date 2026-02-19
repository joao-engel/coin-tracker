using Core.Lib.Domain.Entities;
using Core.Lib.DTOs;
using Core.Lib.Infra.Mensageria;
using Core.Lib.Repositories;
using Worker.Crypto.Integration;

namespace Worker.Crypto;

public class CryptoWorker : BackgroundService
{
    private readonly ILogger<CryptoWorker> _logger;
    private readonly RabbitMQService _rabbitMQService;
    private readonly BinanceService _binanceService;
    private readonly IServiceProvider _serviceProvider;

    private const string _exchangeName = "crypto_exchange";

    public CryptoWorker(ILogger<CryptoWorker> logger, RabbitMQService rabbitMQService, BinanceService binanceService, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _rabbitMQService = rabbitMQService;
        _binanceService = binanceService;
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"CryptoWorker (Publisher) iniciado as: {DateTime.Now}");


        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                List<CryptoAsset> activeCoins = [];
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<Repository<CryptoAsset>>();
                    var all = await repo.GetAllAsync();
                    activeCoins = all.Where(x => x.IsActive).ToList();
                }

                List<PriceUpdateDto> coins = await _binanceService.SearchCoins(activeCoins);

                foreach (var coin in coins)
                {
                    await _rabbitMQService.PublishMessageAsync(
                        message: coin,
                        exchangeName: _exchangeName,
                        routingKey: $"coin.{coin.RoutingKey}"
                    );

                    _logger.LogInformation($"[Publisher] Enviado: {coin.Coin} (${coin.Price:F2})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar mensagem");
            }

            await Task.Delay(15000, stoppingToken);
        }
    }
}