import argparse
import json
import multiprocessing
import time
from baking import BakingFurnaceSensor
from conveyor import ConveyorBeltSensor
from dough import DoughMixerSensor
from packing import PackingLineSensor

def run_sensor(sensor_class, sensor_id, broker, port, topic, interval, randomness, wallet_address):
    sensor = sensor_class(sensor_id, broker, port, topic, wallet_address, auto_run=True)
    try:
        sensor.run(interval, randomness)
    except KeyboardInterrupt:
        pass
    finally:
        sensor.disconnect()
        print(f"Sensor {sensor_id} stopped")

def main():
    parser = argparse.ArgumentParser(description = "Run multiple MQTT sensors")
    parser.add_argument("--broker", type=str, default = "localhost", help = "MQTT broker address")
    parser.add_argument("--port", type=int, default = 1883, help = "MQTT broker port")
    parser.add_argument("--interval", type=int, default = 5, help = "Publishing interval in seconds")
    parser.add_argument("--randomness", type=float, default = 1, help = "Randomness added to the interval")
    parser.add_argument("--baking-furnaces", type=int, default = 1, help = "Number of baking furnace sensors")
    parser.add_argument("--conveyor-belts", type=int, default = 1, help = "Number of conveyor belt sensors")
    parser.add_argument("--dough-mixers", type=int, default = 1, help = "Number of dough mixer sensors")
    parser.add_argument("--packing-lines", type=int, default = 1, help = "Number of packing line sensors")
    parser.add_argument("--wallets-file", type=str, default="wallets.json", help="Path to the JSON file with wallet addresses")

    args = parser.parse_args()

    wallets = []
    try:
        with open(args.wallets_file, 'r') as f:
            wallets_data = json.load(f)
            wallets = [wallet['address'] for wallet in wallets_data]
    except FileNotFoundError:
        print(f"Wallets file not found at {args.wallets_file}.")
    except (json.JSONDecodeError, KeyError):
        print(f"Invalid wallets file at {args.wallets_file}.")

    sensors = {
        "baking_furnace": (args.baking_furnaces, BakingFurnaceSensor, "sensors/baking"),
        "conveyor_belt": (args.conveyor_belts, ConveyorBeltSensor, "sensors/conveyor"),
        "dough_mixer": (args.dough_mixers, DoughMixerSensor, "sensors/dough"),
        "packing_line": (args.packing_lines, PackingLineSensor, "sensors/packing"),
    }

    processes = []
    
    wallet_index = 0
    for sensor_name, (count, sensor_class, topic) in sensors.items():
        if count > 0:
            for i in range(count):
                sensor_id = len(processes) + 1
                wallet_address = None
                if wallet_index < len(wallets):
                    wallet_address = wallets[wallet_index]
                    wallet_index += 1
                else:
                    print(f"Not enough wallet addresses for sensor {sensor_name} ID {sensor_id}.")
                
                process = multiprocessing.Process(
                    target = run_sensor,
                    args = (sensor_class, sensor_id, args.broker, args.port, topic, args.interval, args.randomness, wallet_address)
                )
                processes.append(process)
                process.start()

    print(f"Started {len(processes)} sensors. Press Ctrl+C to stop.")

    try:
        while any(p.is_alive() for p in processes):
            time.sleep(1)
    except KeyboardInterrupt:
        print("\nShutting down all sensors")
        for p in processes:
            p.terminate()
            p.join()

if __name__ == "__main__":
    main()