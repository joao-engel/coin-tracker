using Api.Management.Services.Crypto;
using Api.Management.Services.Crypto.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Management.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class CryptoAssetController : ControllerBase
{
    private readonly ILogger<CryptoAssetController> _logger;
    private readonly CryptoAssetService _cryptoAssetService;

    public CryptoAssetController(ILogger<CryptoAssetController> logger, CryptoAssetService cryptoAssetService)
    {
        _logger = logger;
        _cryptoAssetService = cryptoAssetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _cryptoAssetService.GetAll());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar cryptos");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            return Ok(await _cryptoAssetService.GetById(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao buscar crypto pelo id '{id}'");
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CryptoAssetInputDto inputDto)
    {
        try
        {
            return Ok(await _cryptoAssetService.Create(inputDto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar crypto");
            throw;
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] Guid id, [FromBody] bool isActive)
    {
        try
        {
            return Ok(await _cryptoAssetService.ToggleStatus(id, isActive));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao desativar crypto pelo id '{id}'");
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CryptoAssetPutDto dto)
    {
        try
        {
            return Ok(await _cryptoAssetService.Update(id, dto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao atualizar crypto pelo id '{id}'");
            throw;
        }
    }
}