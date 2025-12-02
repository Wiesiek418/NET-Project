using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Domains.Sensors.Application;
using Domains.Sensors.Models;
using Infrastructure.Mqtt;
using Infrastructure.SignalR;

namespace Domains.Sensors.EventHandlers;

public class SensorHandler<TReading> : IMqttMessageHandler<TReading> 
    where TReading : SensorReading
{
    private readonly ILogger<SensorHandler<TReading>> _logger;
    private readonly SensorService _sensorService;
    private readonly IHubContext<NotificationHub> _hub;

    public SensorHandler(
        ILogger<SensorHandler<TReading>> logger,
        SensorService sensorService,
        IHubContext<NotificationHub> hub)
    {
        _logger = logger;
        _sensorService = sensorService;
        _hub = hub;
    }

    public async Task HandleAsync(TReading reading, CancellationToken ct)
    {
        _logger.LogDebug("Received {ReadingType} reading: {@Reading}", typeof(TReading).Name, reading);
        
        await _sensorService.SaveReadingAsync(reading, ct);

        await _hub.Clients.All.SendAsync("NewReading", reading, ct);
    }
}