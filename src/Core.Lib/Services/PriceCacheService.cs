using Core.Lib.DTOs;
using Core.Lib.Infra.Cache;
using System.Text.Json;

namespace Core.Lib.Services;

public class PriceCacheService
{
    private readonly RedisService _redis;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private const string KeyPrefix = "price:curr:";

    public PriceCacheService(RedisService redis)
    {
        _redis = redis;
    }

    public async Task UpdatePrice(string message)
    {
        PriceUpdateDto price = JsonSerializer.Deserialize<PriceUpdateDto>(message, _jsonSerializerOptions)
                ?? throw new Exception("Erro ao deserializar mensagem");

        string key = GetKey(price.RoutingKey);
        await _redis.SetAsync(key, price, TimeSpan.FromMinutes(1));
    }

    public async Task<PriceUpdateDto?> GetPriceAsync(string symbol)
    {
        var key = GetKey(symbol);
        return await _redis.GetAsync<PriceUpdateDto>(key);
    }

    public async Task<List<PriceUpdateDto>> GetAllPricesAsync()
    {
        return await _redis.GetByPrefixAsync<PriceUpdateDto>(KeyPrefix);
    }

    private static string GetKey(string symbol) => $"{KeyPrefix}{symbol.ToLower()}";
}
