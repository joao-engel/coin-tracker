using Core.Lib.Configuration;
using Core.Lib.DTOs;
using StackExchange.Redis;
using System.Text.Json;

namespace Core.Lib.Services;

public class PriceCacheService
{
    private readonly IConnectionMultiplexer _redis;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private const string KeyPrefix = "price:curr:";

    public PriceCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task UpdatePrice(string message)
    {
        PriceUpdateDto price = JsonSerializer.Deserialize<PriceUpdateDto>(message, _jsonSerializerOptions)
                ?? throw new Exception("Erro ao deserializar mensagem");

        var db = _redis.GetDatabase();

        string key = GetKey(price.Coin);
        string json = JsonSerializer.Serialize(price);
        await db.StringSetAsync(key, json, TimeSpan.FromSeconds(60));
    }

    public async Task<PriceUpdateDto?> GetPriceAsync(string symbol)
    {
        var db = _redis.GetDatabase();

        var key = GetKey(symbol);
        var json = await db.StringGetAsync(key);

        if (json.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<PriceUpdateDto>(json.ToString());
    }

    public async Task<List<PriceUpdateDto>> GetAllPricesAsync()
    {
        var db = _redis.GetDatabase();

        List<PriceUpdateDto> result = [];

        foreach (var coin in CryptoCatalog.Coins)
        {
            var key = $"price:curr:{coin.RoutingKey}";
            var json = await db.StringGetAsync(key);

            if (json.IsNullOrEmpty)
                continue;

            var dto = JsonSerializer.Deserialize<PriceUpdateDto>(json.ToString());
            
            if (dto != null) 
                result.Add(dto);
        }

        return result;
    }

    private static string GetKey(string symbol) => $"{KeyPrefix}{symbol.ToLower()}";
}
