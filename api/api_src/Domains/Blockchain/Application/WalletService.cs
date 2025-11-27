using Core.Abstractions;
using Domains.Blockchain.Infrastructure;
using Domains.Blockchain.Infrastructure.Data;
using Domains.Blockchain.Models;
using Nethereum.Web3;
using System.Numerics;
using Microsoft.Extensions.Options;

namespace Domains.Blockchain.Application;

/// <summary>
/// Application service for wallet operations.
/// </summary>
public class WalletService
{
    private readonly WalletUnitOfWork _unitOfWork;
    private readonly BlockchainSettings _settings;
    private Web3 _web3;
    private readonly string _abi = @"[
        {
            ""constant"": true,
            ""inputs"": [{ ""name"": ""account"", ""type"": ""address"" }],
            ""name"": ""balanceOf"",
            ""outputs"": [{ ""name"": """", ""type"": ""uint256"" }],
            ""type"": ""function""
        }
    ]";

    public WalletService(
        WalletUnitOfWork unitOfWork, 
        IOptions<BlockchainSettings> _settings)
    {
        _unitOfWork = unitOfWork;
        this._settings = _settings.Value;
        _web3 = new Web3(this._settings.RpcUrl);
    }

    public async Task<IEnumerable<WalletInfo>> GetAllWalletsAsync(CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<WalletInfo>();
        return await repository.GetAllAsync(ct);
    }

    public async Task<IEnumerable<WalletBalance>> GetBalancesAsync(bool raw = false, CancellationToken ct = default)
    {
        var wallets = await GetAllWalletsAsync(ct);
        var contract = _web3.Eth.GetContract(_abi, _settings.TokenAddress);
        var balanceOf = contract.GetFunction("balanceOf");

        // For testing purposes, if no wallets are found, use a default one
        if (!wallets.Any()) {
            wallets = new List<WalletInfo> 
            {
                new WalletInfo { Id = "1", Address = "0xe3587046989c92D55bA245E6DC941EE47C479a92", SensorType = "SensorA" }
            };
        }

        var res = wallets.Select(async w => 
        {
            BigInteger rawValue = await balanceOf.CallAsync<BigInteger>(w.Address);
            double balance = raw ? (double)rawValue : (double)rawValue / Math.Pow(10, _settings.TokenDecimals);

            return new WalletBalance
            {
                Id = w.Id,
                SensorType = w.SensorType,
                Balance = balance
            };
        });

        return await Task.WhenAll(res);
    }
}
