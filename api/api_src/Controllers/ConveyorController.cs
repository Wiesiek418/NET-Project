using Microsoft.AspNetCore.Mvc;

using Data.Services.Interfaces;
using Data.Models;

namespace Controllers;

[ApiController]
[Route("api2/[controller]")]
public class ConveyorController : ControllerBase
{
    private readonly IConveyorService _service;

    public ConveyorController(IConveyorService service) =>
        _service = service;

    [HttpGet]
    public async Task<IEnumerable<ConveyorBeltReading>> Get() =>
        await _service.GetAllAsync();

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ConveyorBeltReading reading)
    {
        if (reading == null) return BadRequest();
        await _service.CreateAsync(reading);
        return CreatedAtAction(nameof(Get), null);
    }
}
