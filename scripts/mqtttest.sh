#!/bin/sh
# Example MQTT publish loop
BROKER="mqtt"
PORT=1883

mosquitto_pub -h $BROKER -p $PORT -t "sensors/conveyor" -m '{"SensorId": 123,"Timestamp": "1337-04-01T00:00:00","Status": "active","BearingTemp": 23.5,"Speed": 12.7}'

mosquitto_pub -h $BROKER -p $PORT -t "sensors/baking" -m '{"SensorId": 124,"Timestamp": "1337-04-01T00:00:00", "Temperature": 185.3, "Humidity": 22.5, "GasFlow": 14.8, "DoorStatus": "open", "Status": "baking" }'

mosquitto_pub -h $BROKER -p $PORT -t "sensors/dough" -m '{"SensorId": 125,"Timestamp": "1337-04-01T00:00:00", "RotationSpeed": 120.5, "MotorTemperature": 56.2, "VibrationLevel": 0.8, "LoadWeight": 48.7, "Status": "idle"}'

mosquitto_pub -h $BROKER -p $PORT -t "sensors/packing" -m '{"SensorId": 126,"Timestamp": "1337-04-01T00:00:00", "ConveyorSpeed": 1.8, "PackageCount": 320, "SealTemperature": 145.6, "ErrorCount": 2, "Status": "running"}'

echo "All messages sent!"