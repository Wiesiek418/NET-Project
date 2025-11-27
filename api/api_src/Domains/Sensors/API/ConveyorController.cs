using Microsoft.AspNetCore.Mvc;
using Domains.Sensors.Models;
using Domains.Sensors.Application;

namespace Domains.Sensors.API;

[ApiController]
[Route("api/[controller]")]
public class ConveyorController : ControllerBase
{
    private readonly SensorService _sensorService;

    public ConveyorController(SensorService sensorService) =>
        _sensorService = sensorService;

    [HttpGet]
    public async Task<IEnumerable<ConveyorBeltReading>> Get(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        CancellationToken ct) =>
        await _sensorService.GetAllConveyorReadingsAsync(filter, sort, ct);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ConveyorBeltReading reading, CancellationToken ct)
    {
        if (reading == null) return BadRequest();
        await _sensorService.SaveConveyorReadingAsync(reading, ct);
        return CreatedAtAction(nameof(Get), null);
    }
}

