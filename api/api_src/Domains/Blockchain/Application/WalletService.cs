using System.Numerics;
using Domains.Blockchain.Infrastructure;
using Domains.Blockchain.Infrastructure.Data;
using Domains.Blockchain.Models;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Eth.DTOs; 
using Nethereum.Hex.HexTypes;

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
        },
        {
            ""constant"": false,
            ""inputs"": [
                { ""name"": ""_to"", ""type"": ""address"" },
                { ""name"": ""_value"", ""type"": ""uint256"" }
            ],
            ""name"": ""transfer"",
            ""outputs"": [{ ""name"": """", ""type"": ""bool"" }],
            ""type"": ""function""
        }
    ]";

    private readonly Account _appAccount;

    private readonly BlockchainSettings _settings;
    private readonly WalletUnitOfWork _unitOfWork;
    private readonly Web3 _web3;
    private readonly NonceService _nonceService;

    public WalletService(
        WalletUnitOfWork unitOfWork,
        IOptions<BlockchainSettings> _settings,
        NonceService nonceService)
    {
        _unitOfWork = unitOfWork;
        this._settings = _settings.Value;
        _nonceService = nonceService;
        
        _appAccount = new Account(this._settings.AppWalletPrivateKey);
        _web3 = new Web3(_appAccount, this._settings.RpcUrl);
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

        Console.WriteLine($"Fetching balances for {wallets.Count()} wallets.");

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

    public async Task<string> SendTokenAsync(string receiverAddress, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(receiverAddress))
            throw new ArgumentException("Receiver address cannot be null or empty.", nameof(receiverAddress));
        
        var web3 = _web3;
        var tokenContract = web3.Eth.GetContract(_abi, _settings.TokenAddress);
        var transferFunction = tokenContract.GetFunction("transfer");

        var tokenAmount = new BigInteger(_settings.PaymentAmount * (decimal)Math.Pow(10, _settings.TokenDecimals));

        var gasEstimate = await transferFunction.EstimateGasAsync(
            _appAccount.Address, 
            null, 
            null, 
            receiverAddress, 
            tokenAmount);
        
        var currentGasPrice = await web3.Eth.GasPrice.SendRequestAsync();
        var bufferedGasPrice = currentGasPrice.Value * 120 / 100; 
        var gasPrice = new HexBigInteger(bufferedGasPrice);

        var transactionInput = transferFunction.CreateTransactionInput(
            _appAccount.Address, 
            gasEstimate, 
            null,
            receiverAddress, 
            tokenAmount);

        transactionInput.Nonce = await _nonceService.GetNextNonceAsync(_appAccount.Address);
        transactionInput.GasPrice = gasPrice;
    
        var txHash = await _web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);

        return txHash;
    }
}