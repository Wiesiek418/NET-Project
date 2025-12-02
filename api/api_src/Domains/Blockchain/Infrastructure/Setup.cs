using Domains.Blockchain.Infrastructure;
using Domains.Blockchain.Infrastructure.Data;
using Domains.Blockchain.Application;

namespace Domains.Blockchain.Infrastructure;

public static class BlockchainInfrastructureSetup
{
    public static WebApplicationBuilder AddBlockchainInfrastructure(this WebApplicationBuilder builder)
    {
        // Register domain settings
        builder.Services.Configure<BlockchainSettings>(
            builder.Configuration.GetSection("Blockchain"));

        // Register infrastructure
        builder.Services.AddScoped<WalletMongoContext>();
        builder.Services.AddScoped(sp =>
            new WalletUnitOfWork(sp.GetRequiredService<WalletMongoContext>()));

        // Register application services
        builder.Services.AddSingleton<NonceService>();
        builder.Services.AddScoped<WalletService>();

        return builder;
    }
}