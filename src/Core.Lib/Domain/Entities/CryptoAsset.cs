namespace Core.Lib.Domain.Entities;

public class CryptoAsset
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public CryptoAsset() { }
}
