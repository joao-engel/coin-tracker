using Core.Lib.Domain.Entities;

namespace Api.Management.Services.Crypto.DTOs;
public class CryptoAssetInputDto
{
    public required string Key { get; set; }
    public required string Symbol { get; set; }
    public required string DisplayName { get; set; }

    public CryptoAsset ToDomain()
    {
        return new CryptoAsset
        {
            Key = this.Key,
            Symbol = this.Symbol,
            DisplayName = this.DisplayName,
            IsActive = true
        };
    }
}
    