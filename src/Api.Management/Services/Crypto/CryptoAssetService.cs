using Api.Management.Services.Crypto.DTOs;
using Core.Lib.Domain.Entities;
using Core.Lib.Infra.Cache;
using Core.Lib.Repositories;

namespace Api.Management.Services.Crypto;

public class CryptoAssetService
{
    private readonly Repository<CryptoAsset> _cryptoAssetRepository;
    private readonly RedisService _redisService;
    private readonly string KeyPrefix = "crypto:asset:";

    public CryptoAssetService(Repository<CryptoAsset> cryptoAssetRepository, RedisService redisService)
    {
        _cryptoAssetRepository = cryptoAssetRepository;
        _redisService = redisService;
    }

    public async Task<List<CryptoAssetOutputDto>> GetAll()
    {
        List<CryptoAsset> cryptoAssets = await _cryptoAssetRepository.GetAllAsync();

        List<CryptoAssetOutputDto> cryptoAssetDtos = [.. cryptoAssets.Select(cryptoAsset => new CryptoAssetOutputDto(cryptoAsset))];
        
        return cryptoAssetDtos;
    }

    public async Task<CryptoAssetOutputDto> GetById(Guid id)
    {
        CryptoAsset cryptoAsset = await _cryptoAssetRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"CryptoAsset com o id '{id}' não foi encontrada.");

        return new CryptoAssetOutputDto(cryptoAsset);
    }

    public async Task<CryptoAssetOutputDto> ToggleStatus(Guid id, bool isActive)
    {
        CryptoAsset entity = await _cryptoAssetRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"CryptoAsset com o id '{id}' não foi encontrada.");

        entity.IsActive = isActive;
        await _cryptoAssetRepository.UpdateAsync(entity);

        await SaveCache(entity);

        return new CryptoAssetOutputDto(entity);
    }

    public async Task<CryptoAssetOutputDto> Create(CryptoAssetInputDto inputDto)
    {
        CryptoAsset entity = inputDto.ToDomain();

        await _cryptoAssetRepository.AddAsync(entity);

        await SaveCache(entity);

        return new CryptoAssetOutputDto(entity);
    }

    public async Task<CryptoAssetOutputDto> Update(Guid id, CryptoAssetPutDto dto)
    {
        CryptoAsset entity = await _cryptoAssetRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"CryptoAsset com o id '{id}' não foi encontrada.");

        dto.UpdateDomain(entity);        
        await _cryptoAssetRepository.UpdateAsync(entity);

        await SaveCache(entity);

        return new CryptoAssetOutputDto(entity);
    }

    private async Task SaveCache(CryptoAsset entity)
    {
        if (entity.IsActive)
            await _redisService.SetAsync($"{KeyPrefix}{entity.Symbol}", entity);
        else
            await _redisService.RemoveAsync($"{KeyPrefix}{entity.Symbol}");        
    }        
}