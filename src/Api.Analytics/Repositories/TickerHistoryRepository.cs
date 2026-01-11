using Api.Analytics.Infra;
using Core.Lib.DTOs;

namespace Api.Analytics.Repositories;

public class TickerHistoryRepository
{
    private readonly DbSession _dbSession;

    public TickerHistoryRepository(DbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<IEnumerable<PriceUpdateDto>> GetPriceHistoryPerSymbol(string coin, DateTime dateTime)
    {
        var sql = $@"
            SELECT 
                ""Coin"", 
                ""Price"",
                ""Timestamp""
            FROM ""Crypto"".""MarketHistory""
            WHERE ""Coin"" = @Coin
            AND ""Timestamp"" >= @DataCorte
            ORDER BY ""Timestamp"" ASC;
        ";

        var objectParameters = new
        {
            Coin = coin,
            DataCorte = dateTime
        };

        return await _dbSession.QueryAsync<PriceUpdateDto>(sql, objectParameters);
    }
}
