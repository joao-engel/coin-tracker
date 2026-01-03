using Core.Lib.Infra;
using Core.Lib.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Worker.PriceProcessor.Services;

namespace Worker.PriceProcessor;

public class PriceWorker(ILogger<PriceWorker> logger, RabbitMQService rabbitService, IConfiguration configuration, IServiceScopeFactory scopeFactory) : BackgroundService
{
    private IChannel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var exchangeName = configuration["RabbitMQSettings:ExchangeName"]
            ?? throw new Exception("ExchangeName is not configured");
        var queueName = configuration["RabbitMQSettings:QueueName"]
            ?? throw new Exception("QueueName is not configured");
        var routingKey = configuration["RabbitMQSettings:RoutingKey"]
            ?? throw new Exception("RoutingKey is not configured");

        logger.LogInformation($"Iniciando Worker para Moeda: {routingKey} na Fila: {queueName}");

        _channel = await rabbitService.SetupChannel(
            exchangeName: exchangeName,
            queueName: queueName,
            routingKey: routingKey
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var historyService = scope.ServiceProvider.GetRequiredService<MarketHistoryService>();
                    var cacheService = scope.ServiceProvider.GetRequiredService<PriceCacheService>();

                    await historyService.SavePrice(message);
                    await cacheService.UpdatePrice(message);
                }                    

                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao processar.");
            }
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}
