using System.Globalization;

namespace Core.Lib.DTOs;

public class PriceUpdateDto
{
    public string Coin { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }

    public PriceUpdateDto() { }

    public PriceUpdateDto(string coin, string routingKey, decimal price)
    {
        Coin = coin;
        RoutingKey = routingKey;
        Price = price;
        Timestamp = DateTime.Now;
    }

    public PriceUpdateDto(string coin, string routingKey, string price)
    {
        if (!decimal.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice))
            throw new ArgumentException("Formato do preço invalido", nameof(price));

        Coin = coin;
        RoutingKey = routingKey;
        Price = parsedPrice;
        Timestamp = DateTime.Now;
    }
}
