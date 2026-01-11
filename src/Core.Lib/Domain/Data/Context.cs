using Core.Lib.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Lib.Domain.Data;
public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<MarketHistory> MarketHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Crypto");

        modelBuilder.ApplyConfiguration(new MarketHistoryContext());
        modelBuilder.ApplyConfiguration(new CryptoAssetContext());

        base.OnModelCreating(modelBuilder);
    }
}
