# NET-Project

## Development

### Backend
Get .NET 9 SDK from [here](https://dotnet.microsoft.com/en-us/download).  
You already have Docker, right ?

## Testing

### Publish to MQTT broker with only Docker

Interactively 
```bash
docker run --network=net-project_default -it --rm efrecon/mqtt-client sh

# inside the container
mosquitto_pub -h mqtt -p 1883 -t "sensors/conveyor" -m '{"SensorId": 123,"Timestamp": "1337-04-01T00:00:00","Status": "active","BearingTemp": 23.5,"Speed": 12.7}'
# ...
exit
```

Script that sends 1 message per sensor
```bash
docker run --rm -v ${PWD}/scripts/mqtttest.sh:/script.sh --network=net-project_default efrecon/mqtt-client sh /script.sh
```