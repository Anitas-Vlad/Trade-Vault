using Microsoft.EntityFrameworkCore;
using TradeVault.Responses;

namespace TradeVault.Context;

public class TradeVaultContext : DbContext
{
    public TradeVaultContext(DbContextOptions<TradeVaultContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ModelBuilderExtensions.Seed(modelBuilder);
    }

    public DbSet<PriceResponse> PriceResponses { get; set; } = default!;

    private static class ModelBuilderExtensions
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
        }
    }
}