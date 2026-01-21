using Core.Lib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Lib.Domain.Data;
public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<MarketHistory> MarketHistory { get; set; }
    public DbSet<CryptoAsset> CryptoAssets { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveConversion<UtcValueConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Crypto");

        modelBuilder.ApplyConfiguration(new MarketHistoryContext());
        modelBuilder.ApplyConfiguration(new CryptoAssetContext());

        base.OnModelCreating(modelBuilder);
    }
}

internal class UtcValueConverter : ValueConverter<DateTime, DateTime>
{
    public UtcValueConverter() : base(
        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
    }
}
