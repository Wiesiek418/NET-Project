using System.Numerics;
using Domains.Blockchain.Infrastructure;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Domains.Blockchain.Application;

public class NonceService
{
    private readonly Web3 _web3;
    private BigInteger? _currentNonce;
    // We use a lock to ensure only one thread calculates the nonce at a time
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public NonceService(IOptions<BlockchainSettings> settings)
    {
        _web3 = new Web3(settings.Value.RpcUrl);
    }

    public async Task<HexBigInteger> GetNextNonceAsync(string address)
    {
        await _semaphore.WaitAsync();
        try
        {
            // If we don't have a nonce yet, or we were told to reset, fetch from chain
            if (!_currentNonce.HasValue)
            {
                // IMPORTANT: Use BlockParameter.CreatePending() to account for txs in the mempool
                var nonce = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address, BlockParameter.CreatePending());
                _currentNonce = nonce.Value;
            }
            else
            {
                _currentNonce++;
            }

            return new HexBigInteger(_currentNonce.Value);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Call this when the blockchain rejects our nonce (e.g. "Nonce too low")
    /// to force a refresh from the network on the next call.
    /// </summary>
    public async Task ResetAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentNonce = null;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}