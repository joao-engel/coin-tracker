using Core.Lib.Domain.Data;
using Core.Lib.Domain.Entities;
using Core.Lib.DTOs;
using System.Text.Json;

namespace Worker.PriceProcessor.Services
{
    public class MarketHistoryService(Context context, ILogger<MarketHistoryService> logger)
    {
        private readonly Context _context = context;
        private readonly ILogger<MarketHistoryService> _logger = logger;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task SavePrice(string message)
        {
            PriceUpdateDto price = JsonSerializer.Deserialize<PriceUpdateDto>(message, _jsonSerializerOptions)
                ?? throw new Exception("Erro ao deserializar mensagem");

            MarketHistory entity = new(price);

            _context.Set<MarketHistory>().Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogDebug($"[DB] Inserido no banco: {entity.Coin} - ${entity.Price}");
        }
    }
}
