using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TradeVault.Context;

public class TradeVaultContextFactory : IDesignTimeDbContextFactory<TradeVaultContext>
{
    public TradeVaultContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Get the connection string
        var connectionString = configuration.GetConnectionString("trade-vault-context");

        // Configure the DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<TradeVaultContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new TradeVaultContext(optionsBuilder.Options);
    }
}