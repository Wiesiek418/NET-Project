using Microsoft.AspNetCore.Mvc;

using Data.Services.Interfaces;
using Data.Models;

namespace Controllers;

[ApiController]
[Route("api2/[controller]")]
public class DoughController : ControllerBase
{
    private readonly IDoughService _service;

    public DoughController(IDoughService service) =>
        _service = service;

    [HttpGet]
    public async Task<IEnumerable<DoughMixerReading>> Get() =>
        await _service.GetAllAsync();

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DoughMixerReading reading)
    {
        if (reading == null) return BadRequest();
        await _service.CreateAsync(reading);
        return CreatedAtAction(nameof(Get), null);
    }
}
