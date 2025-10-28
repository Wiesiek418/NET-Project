# NET-Project

## API

### Alpha
Test data flow for a sensor named Alpha:
- subscribes to topic "sensors/alpha"
- handles AlphaReading entities - { Value: int }
- exposes endpoint for GETing all AlphaReadings from db - at "http://localhost:5000/api2/alpha"

## Development

### Backend
Get .NET 9 SDK from [here](https://dotnet.microsoft.com/en-us/download).  
You already have Docker, right ?

## Testing

### Publish to MQTT broker with Docker only
```bash
docker run --network=net-project_default -it --rm efrecon/mqtt-client pub -h mqtt -p 1883 -t "sensors/alpha" -m '{"Value": 501}'
```
This creates a new container, adds it to the default compose network and publishes the message to topic "sensors/alpha".  
Useful for testing the backend without setting up a real publisher.