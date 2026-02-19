using Api.Analytics.Repositories;
using Core.Lib.Domain.Entities;
using Core.Lib.DTOs;
using Core.Lib.Infra.Cache;

namespace Api.Analytics.Services;
public class TickerHistoryService
{
    private readonly TickerHistoryRepository _repository;
    private readonly RedisService _redis;

    public TickerHistoryService(TickerHistoryRepository repository, RedisService redis)
    {
        _repository = repository;
        _redis = redis;
    }

    public async Task<IEnumerable<PriceUpdateDto>> GetPriceHistoryPerSymbol(string key, int hours)
    {
        var dateTime = DateTime.UtcNow.AddHours(-hours);

        CryptoAsset coin = await _redis.GetAsync<CryptoAsset>($"crypto:asset:{key}")
            ?? throw new KeyNotFoundException($"CryptoAsset com o id '{key}' não foi encontrada.");

        return await _repository.GetPriceHistoryPerSymbol(coin.DisplayName, dateTime);
    }
}
