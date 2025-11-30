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
            if (!_currentNonce.HasValue)
            {
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

    public void Reset()
    {
        _currentNonce = null;
    }
}