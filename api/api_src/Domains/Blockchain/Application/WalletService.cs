using System.Numerics;
using Domains.Blockchain.Infrastructure;
using Domains.Blockchain.Infrastructure.Data;
using Domains.Blockchain.Models;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

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
    private readonly NonceService _nonceService;

    private readonly BlockchainSettings _settings;
    private readonly WalletUnitOfWork _unitOfWork;
    private readonly Web3 _web3;

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

    public async Task<IEnumerable<WalletBalance>> GetBalancesAsync(
        bool raw = false,
        CancellationToken ct = default)
    {
        var batchSize = 50;
        var wallets = (await GetAllWalletsAsync(ct)).ToList();
        var contract = _web3.Eth.GetContract(_abi, _settings.TokenAddress);
        var balanceOf = contract.GetFunction("balanceOf");

        Console.WriteLine($"Fetching balances for {wallets.Count} wallets (batch size {batchSize}).");

        // For testing purposes, if no wallets are found, use a default one
        if (!wallets.Any())
            wallets = new List<WalletInfo>
            {
                new() { Id = "1", Address = "0xe3587046989c92D55bA245E6DC941EE47C479a92", SensorType = "SensorA" }
            };

        var walletList = wallets;
        var resultArray = new WalletBalance[walletList.Count];

        for (var i = 0; i < walletList.Count; i += batchSize)
        {
            ct.ThrowIfCancellationRequested();

            var batch = walletList
                .Skip(i)
                .Take(batchSize)
                .Select((w, idx) => new { Wallet = w, Index = i + idx })
                .ToList();

            var tasks = batch.Select(async item =>
            {
                while (true)
                    try
                    {
                        var rawValue = await balanceOf.CallAsync<BigInteger>(item.Wallet.Address).ConfigureAwait(false);
                        var balance = raw
                            ? (double)rawValue
                            : (double)rawValue / Math.Pow(10, _settings.TokenDecimals);

                        resultArray[item.Index] = new WalletBalance
                        {
                            Id = item.Wallet.Id,
                            SensorId = item.Wallet.SensorId,
                            SensorType = item.Wallet.SensorType,
                            Balance = balance
                        };
                        return;
                    }
                    catch (Exception ex) when (ex is RpcClientUnknownException || ex is RpcResponseException)
                    {
                        Console.WriteLine("Error while downloading balance. Retrying...");
                        await Task.Delay(500, ct);
                    }
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        return resultArray;
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
        while (true)
            try
            {
                return await SendTokenInternalAsync(receiverAddress, ct);
            }
            catch (Exception ex) when (ex is RpcClientUnknownException || ex is RpcResponseException)
            {
                await _nonceService.ResetAsync();
                Console.WriteLine("Blockchain provider error - trying again in two seconds \n\n");
                await Task.Delay(2000, ct);
            }
    }

    private async Task<string> SendTokenInternalAsync(string receiverAddress, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(receiverAddress)) throw new ArgumentException("Address empty");

        var contract = _web3.Eth.GetContract(_abi, _settings.TokenAddress);
        var transferFunction = contract.GetFunction("transfer");
        var tokenAmount = new BigInteger(_settings.PaymentAmount * (decimal)Math.Pow(10, _settings.TokenDecimals));

        var gasLimit = new HexBigInteger(100000);

        var currentGasPrice = await _web3.Eth.GasPrice.SendRequestAsync();
        var gasPrice = new HexBigInteger(currentGasPrice.Value * 120 / 100);

        var transactionInput = transferFunction.CreateTransactionInput(
            _appAccount.Address,
            gasLimit,
            null,
            receiverAddress,
            tokenAmount);

        transactionInput.Nonce = await _nonceService.GetNextNonceAsync(_appAccount.Address);
        transactionInput.GasPrice = gasPrice;

        await Task.Delay(500, ct);

        Console.WriteLine("\n\nSEND");
        var txHash = await _web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);
        Console.WriteLine("SUCCESS\n\n");

        return txHash;
    }
}