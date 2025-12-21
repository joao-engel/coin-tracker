using Core.Lib.DTOs;

namespace Core.Lib.Domain.Entities;

public class MarketHistory
{
    public Guid ID { get; set; }
    public string Coin { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime ProcessedAt { get; set; }

    public MarketHistory() { }

    public MarketHistory(PriceUpdateDto dto)
    {
        Coin = dto.Coin;
        Price = dto.Price;
        Timestamp = dto.Timestamp;
        ProcessedAt = DateTime.Now;
    }
}
