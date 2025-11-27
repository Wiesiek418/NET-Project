using Domains.Blockchain.Application;
using Domains.Blockchain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domains.Blockchain.API;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly WalletService _service;

    public WalletController(WalletService service)
    {
        _service = service;
    }

    // GET /api/wallet/balances?raw=true
    [HttpGet("balances")]
    public async Task<IEnumerable<WalletBalance>> GetBalances([FromQuery] bool raw = false)
    {
        return await _service.GetBalancesAsync(raw);
    }

    // GET /api/wallet
    [HttpGet]
    public async Task<IEnumerable<WalletInfo>> GetAllWallets()
    {
        return await _service.GetAllWalletsAsync();
    }
}