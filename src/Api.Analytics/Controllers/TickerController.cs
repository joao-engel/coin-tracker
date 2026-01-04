using Core.Lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Analytics.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class TickerController : Controller
{
    private readonly ILogger<TickerController> _logger;
    private readonly PriceCacheService _priceCacheService;

    public TickerController(ILogger<TickerController> logger, PriceCacheService priceCacheService)
    {
        _logger = logger;
        _priceCacheService = priceCacheService;
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
}
