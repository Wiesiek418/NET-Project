using Microsoft.AspNetCore.Mvc;

using Data.Services.Interfaces;
using Data.Models;

namespace Controllers;

[ApiController]
[Route("api2/[controller]")]
public class AlphaController : ControllerBase
{
    private readonly IAlphaService _service;

    public AlphaController(IAlphaService service) =>
        _service = service;

    [HttpGet]
    public async Task<IEnumerable<AlphaReading>> Get() =>
        await _service.GetAllAsync();
}