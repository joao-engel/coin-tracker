using Core.Lib.Configuration;
using Core.Lib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Lib.Domain.Data;

public class CryptoAssetContext : IEntityTypeConfiguration<CryptoAsset>
{
    public void Configure(EntityTypeBuilder<CryptoAsset> builder)
    {
        builder.ToTable("CryptoCatalog");

        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(k => k.Key)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(k => k.Symbol)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(k => k.DisplayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(k => k.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(k => k.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.HasIndex(k => k.Key).IsUnique();
        builder.HasIndex(k => k.Symbol).IsUnique();
        builder.HasIndex(k => k.DisplayName).IsUnique();

        builder.HasData(DefaultValues);
    }

    private static readonly IReadOnlyList<CryptoAsset> DefaultValues =
    [
        new CryptoAsset
            {
                Id = Guid.Parse("d8d64c02-4638-4e6f-80d5-53305047f3b1"),
                Key = "btc",
                Symbol = "BTCUSDT",
                DisplayName = "Bitcoin",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CryptoAsset
            {
                Id = Guid.Parse("a5e2f7b8-9c1d-4e2a-8f3b-6c4d9e0a1f2b"),
                Key = "eth",
                Symbol = "ETHUSDT",
                DisplayName = "Ethereum",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CryptoAsset
            {
                Id = Guid.Parse("c1b3d4e5-6f7a-8b9c-0d1e-2f3a4b5c6d7e"),
                Key = "doge",
                Symbol = "DOGEUSDT",
                DisplayName = "Dogecoin",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CryptoAsset
            {
                Id = Guid.Parse("f9e8d7c6-b5a4-3f2e-1d0c-9b8a7f6e5d4c"),
                Key = "sol",
                Symbol = "SOLUSDT",
                DisplayName = "Solana",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new CryptoAsset
            {
                Id = Guid.Parse("e1d2c3b4-a5f6-7e8d-9c0b-1a2f3e4d5c6b"),
                Key = "xrp",
                Symbol = "XRPUSDT",
                DisplayName = "XRP",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
    ];
}
