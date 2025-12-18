namespace Core.Lib.DTOs;

public class PriceUpdateDto
{
    public string Coin { get; set; }
    public string RoutingKey { get; set; }
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }

    public PriceUpdateDto(string coin, string routingKey, decimal price)
    {
        Coin = coin;
        RoutingKey = routingKey;
        Price = price;
        Timestamp = DateTime.Now;
    }

    public PriceUpdateDto(string coin, string routingKey, string price)
    {
        if (!decimal.TryParse(price, out decimal parsedPrice))
            throw new ArgumentException("Formato do preço invalido", nameof(price));

        Coin = coin;
        RoutingKey = routingKey;
        Price = parsedPrice;
        Timestamp = DateTime.Now;
    }
}
