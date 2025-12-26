namespace Core.Lib.Configuration;

public record CryptoRecord(
    string RoutingKey,
    string Symbol,
    string DisplayName
);

public static class CryptoCatalog
{
    public static readonly IReadOnlyList<CryptoRecord> Coins =
    [
        new("btc",  "BTCUSDT",  "Bitcoin"),
        new("eth",  "ETHUSDT",  "Ethereum"),
        new("doge", "DOGEUSDT", "Dogecoin"),
        new("sol",  "SOLUSDT",  "Solana"),
        new("xrp",  "XRPUSDT",  "XRP")
    ];

    public static CryptoRecord? GetByRoutingKey(string key)
        => Coins.FirstOrDefault(c => c.RoutingKey.Equals(key, StringComparison.OrdinalIgnoreCase));

    public static CryptoRecord? GetBySymbol(string symbol)
        => Coins.FirstOrDefault(c => c.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));

    public static CryptoRecord? GetByName(string name)
        => Coins.FirstOrDefault(c => c.DisplayName.Equals(name, StringComparison.OrdinalIgnoreCase));
}