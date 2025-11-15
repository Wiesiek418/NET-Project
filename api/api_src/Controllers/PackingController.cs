using Microsoft.AspNetCore.Mvc;

using Data.Services.Interfaces;
using Data.Models;

namespace Controllers;

[ApiController]
[Route("api2/[controller]")]
public class PackingController : ControllerBase
{
    private readonly IPackingService _service;

    public PackingController(IPackingService service) =>
        _service = service;

    [HttpGet]
    public async Task<IEnumerable<PackingLineReading>> Get(
        [FromQuery] string? filter, 
        [FromQuery] string? sort) =>
        await _service.GetAllAsync(filter, sort);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PackingLineReading reading)
    {
        if (reading == null) return BadRequest();
        await _service.CreateAsync(reading);
        return CreatedAtAction(nameof(Get), null);
    }
}
