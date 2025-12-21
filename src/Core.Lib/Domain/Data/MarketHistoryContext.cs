using Core.Lib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Lib.Domain.Data;
public class MarketHistoryContext : IEntityTypeConfiguration<MarketHistory>
{
    public void Configure(EntityTypeBuilder<MarketHistory> builder)
    {
        builder.HasKey(k => k.ID);
        builder.Property(k => k.ID)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(k => k.Coin)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(k => k.Price)
            .IsRequired()
            .HasPrecision(18, 8);

        builder.HasIndex(p => p.Coin);
        builder.HasIndex(p => p.Timestamp);
    }
}