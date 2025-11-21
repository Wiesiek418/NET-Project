# MQTT Sensors

## Prerequisites

Before running the sensors, ensure you have the necessary Python packages installed:

```bash
pip install paho-mqtt
```

## Running a single sensor

Each sensor script can be run independently from the command line.   
The arguments for each sensor are:
- `--id`: Unique identifier (int) for the sensor instance (required).
- `--broker`: Address of the MQTT broker (default: localhost).
- `--port`: Port number of the MQTT broker (default: 1883).
- `--topic`: MQTT topic to publish to (default varies by sensor).
- `--interval`: Publishing interval in seconds (default: 5).
- `--randomness`: Randomness added to the interval (default: 1).

### Example

To run a single baking furnace sensor:

```bash
python baking.py --id 1
```

Press `Ctrl+C` to stop the sensor.

## Running multiple sensors

The `run_all.py` script allows you to start multiple sensor instances of different types at once.   
The arguments for the script are:
- `--broker`: Address of the MQTT broker (default: localhost).
- `--port`: Port number of the MQTT broker (default: 1883).
- `--interval`: Publishing interval in seconds (default: 5).
- `--randomness`: Randomness added to the interval (default: 1).
- `--baking-furnaces`: Number of baking furnace sensors (default: 1).
- `--conveyor-belts`: Number of conveyor belt sensors (default: 1).
- `--dough-mixers`: Number of dough mixer sensors (default: 1).
- `--packing-lines`: Number of packing line sensors (default: 1).

### Example

To start two baking furnaces, one conveyor belt, one dough mixer, and one packing line:

```bash
python run_all.py --baking-furnaces 2 --conveyor-belts 1 --dough-mixers 1 --packing-lines 1
```

Press `Ctrl+C` to stop all sensors.
