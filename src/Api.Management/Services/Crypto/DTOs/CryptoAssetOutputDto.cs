using Core.Lib.Domain.Entities;

namespace Api.Management.Services.Crypto.DTOs;
public class CryptoAssetOutputDto
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string Simbol { get; set; }
    public string DisplayName { get; set; }
    public bool IsActive { get; set; }

    public CryptoAssetOutputDto(CryptoAsset cryptoAsset)
    {
        Id = cryptoAsset.Id;
        Key = cryptoAsset.Key;
        Simbol = cryptoAsset.Symbol;
        DisplayName = cryptoAsset.DisplayName;
        IsActive = cryptoAsset.IsActive;
    }
}
