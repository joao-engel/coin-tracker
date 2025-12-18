using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Core.Lib.Infra;
public class RabbitMQService
{
    private readonly IConnection _conn;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(IConnection conn, ILogger<RabbitMQService> logger)
    {
        _conn = conn;
        _logger = logger;
    }

    public async Task PublishMessageAsync<T>(T message, string exchangeName, string routingKey)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        using var channel = await _conn.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Direct
        );

        _logger.LogDebug($"Publicando: {jsonMessage}");

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: body
        );

        _logger.LogDebug($"Mensagem enviada para Exchange '{exchangeName}' com sucesso.");
    }

    public async Task<IChannel> SetupChannel(string exchangeName, string queueName, string routingKey)
    {
        _logger.LogInformation($"Configurando fila '{queueName}' na exchange '{exchangeName}'...");

        var channel = await _conn.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: routingKey
        );

        _logger.LogInformation("Topologia configurada com sucesso.");

        return channel;
    }
}
