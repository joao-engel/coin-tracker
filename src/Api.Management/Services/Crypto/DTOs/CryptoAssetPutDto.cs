using Core.Lib.Domain.Entities;

namespace Api.Management.Services.Crypto.DTOs;
public class CryptoAssetPutDto
{
    public string? DisplayName { get; set; }
    public bool? IsActive { get; set; }

    public void UpdateDomain(CryptoAsset cryptoAsset)
    {
        if (!string.IsNullOrEmpty(DisplayName))        
            cryptoAsset.DisplayName = DisplayName;
        
        if (IsActive.HasValue)        
            cryptoAsset.IsActive = IsActive.Value;        
    }
}
