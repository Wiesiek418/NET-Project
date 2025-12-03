using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Domains.Sensors.Application;
using Domains.Sensors.Models;
using Infrastructure.Mqtt;


namespace Domains.Sensors.EventHandlers;

public class SensorHandler<TReading> : IMqttMessageHandler<TReading> 
    where TReading : SensorReading
{
    private readonly ILogger<SensorHandler<TReading>> _logger;
    private readonly SensorService _sensorService;

    public SensorHandler(
        ILogger<SensorHandler<TReading>> logger,
        SensorService sensorService)
    {
        _logger = logger;
        _sensorService = sensorService;
    }

    public async Task HandleAsync(TReading reading, CancellationToken ct)
    {
        _logger.LogDebug("Received {ReadingType} reading: {@Reading}", typeof(TReading).Name, reading);
        
        await _sensorService.SaveReadingAsync(reading, ct);
    }
}