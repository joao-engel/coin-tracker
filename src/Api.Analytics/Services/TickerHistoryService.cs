using Api.Analytics.Repositories;
using Core.Lib.Configuration;
using Core.Lib.DTOs;

namespace Api.Analytics.Services;
public class TickerHistoryService
{
    private readonly TickerHistoryRepository _repository;

    public TickerHistoryService(TickerHistoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PriceUpdateDto>> GetPriceHistoryPerSymbol(string key, int hours)
    {
        var dateTime = DateTime.UtcNow.AddHours(-hours);

        string coin = CryptoCatalog.GetByRoutingKey(key)?.DisplayName 
            ?? throw new ArgumentException("A key informada é inválida");

        return await _repository.GetPriceHistoryPerSymbol(coin, dateTime);
    }
}
