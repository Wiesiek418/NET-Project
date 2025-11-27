using System.Numerics;
using Domains.Blockchain.Infrastructure;
using Domains.Blockchain.Infrastructure.Data;
using Domains.Blockchain.Models;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using System.Security.Cryptography;

namespace Domains.Blockchain.Application;

/// <summary>
///     Application service for wallet operations.
/// </summary>
public class WalletService
{
    private readonly string _abi = @"[
        {
            ""constant"": true,
            ""inputs"": [{ ""name"": ""account"", ""type"": ""address"" }],
            ""name"": ""balanceOf"",
            ""outputs"": [{ ""name"": """", ""type"": ""uint256"" }],
            ""type"": ""function""
        }
    ]";

    private readonly BlockchainSettings _settings;
    private readonly WalletUnitOfWork _unitOfWork;
    private readonly Web3 _web3;

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
        if (!wallets.Any())
            wallets = new List<WalletInfo>
            {
                new() { Id = "1", Address = "0xe3587046989c92D55bA245E6DC941EE47C479a92", SensorType = "SensorA" }
            };

        var res = wallets.Select(async w =>
        {
            var rawValue = await balanceOf.CallAsync<BigInteger>(w.Address);
            var balance = raw ? (double)rawValue : (double)rawValue / Math.Pow(10, _settings.TokenDecimals);

            return new WalletBalance
            {
                Id = w.Id,
                SensorId = w.SensorId,
                SensorType = w.SensorType,
                Balance = balance
            };
        });

        return await Task.WhenAll(res);
    }

    public async Task RegisterOrUpdateSensorAsync(int sensorId, string sensorType, string? walletAddress,
        CancellationToken ct = default)
    {
        var repository = _unitOfWork.GetRepository<WalletInfo>();

        // Checking if sensor already exists
        var filter = $"SensorId:eq:{sensorId},SensorType:eq:{sensorType}";
        var existingWallets = await repository.GetAllAsync(filter, null, ct);
        var walletEntry = existingWallets.FirstOrDefault();

        if (walletEntry == null)
        {
            var newWallet = new WalletInfo
            {
                SensorId = sensorId,
                SensorType = sensorType,
                Address = walletAddress ?? string.Empty
            };
            await repository.CreateAsync(newWallet, ct);
        }
        else if (!string.IsNullOrEmpty(walletAddress) && walletEntry.Address != walletAddress)
        {
            walletEntry.Address = walletAddress;
            await repository.UpdateAsync(walletEntry, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}