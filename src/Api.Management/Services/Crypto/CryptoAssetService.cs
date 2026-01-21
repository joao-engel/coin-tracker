using Api.Management.Services.Crypto.DTOs;
using Core.Lib.Domain.Entities;
using Core.Lib.Repositories;

namespace Api.Management.Services.Crypto;

public class CryptoAssetService
{
    private readonly Repository<CryptoAsset> _cryptoAssetRepository;

    public CryptoAssetService(Repository<CryptoAsset> cryptoAssetRepository)
    {
        _cryptoAssetRepository = cryptoAssetRepository;
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
}