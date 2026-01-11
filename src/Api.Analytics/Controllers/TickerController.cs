using Api.Analytics.Services;
using Core.Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Analytics.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class TickerController : Controller
{
    private readonly ILogger<TickerController> _logger;
    private readonly PriceCacheService _priceCacheService;
    private readonly TickerHistoryService _tickerHistoryService;

    public TickerController(ILogger<TickerController> logger, PriceCacheService priceCacheService, TickerHistoryService tickerHistoryService)
    {
        _logger = logger;
        _priceCacheService = priceCacheService;
        _tickerHistoryService = tickerHistoryService;
    }

    [HttpGet("Latest")]
    public async Task<IActionResult> GetLatestPrices()
    {
        try
        {
            return Ok(await _priceCacheService.GetAllPricesAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar precos");
            throw;
        }
    }

    [HttpGet("History/{key}")]
    public async Task<IActionResult> GetPriceHistoryPerSymbol(string key, [FromQuery] int hours = 24)
    {
        try
        {
            return Ok(await _tickerHistoryService.GetPriceHistoryPerSymbol(key, hours));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao buscar historico de precos para {key}");
            throw;
        }
    }

    [HttpGet("Stream")]
    public async Task GetRealTime(CancellationToken cancellationToken)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        while (!cancellationToken.IsCancellationRequested)
        {
            var prices = await _priceCacheService.GetAllPricesAsync();

            if (prices.Count != 0)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(prices);

                await Response.WriteAsync($"data: {json}\n\n", cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            try
            {
                await Task.Delay(15000, cancellationToken);

            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }
}
