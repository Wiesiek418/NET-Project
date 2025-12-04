
import argparse
import json
import os
import random
import time
import paho.mqtt.client as mqtt
from datetime import datetime

class BaseSensor:
    def __init__(self, sensor_id, broker_address, port, topic, wallet_address, auto_run = False):
        self.sensor_id = sensor_id
        self.auto_run = auto_run
        self.broker_address = broker_address
        self.port = port
        self.topic = topic
        self.wallet_address = wallet_address
        self.name = f"{topic}-sensor-{sensor_id}"
        self.client = mqtt.Client(client_id = self.name)
        self.client.on_connect = self.on_connect
        self.client.on_disconnect = self.on_disconnect
        self.stop = False

    def on_connect(self, client, userdata, flags, rc):
        if rc == 0:
            print(f"Sensor {self.name} connected")
        else:
            print(f"Sensor {self.name} failed to connect, result: {rc}\n")

    def on_disconnect(self, client, userdata, rc):
        print(f"Sensor {self.name} disconnected, result: {rc}")

    def connect(self):
        self.client.connect(self.broker_address, self.port)
        self.client.loop_start()

    def disconnect(self):
        self.client.loop_stop()
        self.client.disconnect()

    def _generate_data(self):
        raise NotImplementedError("This is base sensor!")

    def _load_overrides(self):
        overrides_path = "overrides.json"
        try:
            overrides_path = os.path.abspath(overrides_path)
            if not os.path.exists(overrides_path):
                return {}
            with open(overrides_path, "r") as f:
                data = json.load(f)

            if isinstance(data, list):
                for item in data:
                    try:
                        if isinstance(item, dict) and int(item.get("id")) == int(self.sensor_id):
                            if item.get("ManualOnly", False) and self.auto_run:
                                self.stop = True
                                continue
                            return {k: v for k, v in item.items() if k != "id" and k != "ManualOnly"}
                    except Exception:
                        continue
            return {}
        except Exception as e:
            print(f"{self.name}: Failed to load overrides.json: {e}")
            return {}

    def run(self, interval, randomness=0):
        self.connect()
        while True:
            data = self._generate_data()
            payload = {
                "SensorId": self.sensor_id,
                "Timestamp": datetime.now().isoformat(),
                "WalletAddress": self.wallet_address,
                **data
            }

            overrides = self._load_overrides()
            if self.stop:
                print(f"Sensor {self.name} is set to manual only")
                break
            if overrides:
                payload.update(overrides)

            payload_json = json.dumps(payload)
            self.client.publish(self.topic, payload_json)
            print(f"Published from {self.name}: {payload_json}")
            sleep_time = interval + random.uniform(-randomness, randomness)
            sleep_time = max(0.1, sleep_time)
            time.sleep(sleep_time)

def main(sensor_class, topic_suffix):
    parser = argparse.ArgumentParser(description=f"Run {sensor_class.__name__}.")
    parser.add_argument("--id", type = int, required = True, help = "Sensor ID")
    parser.add_argument("--broker", type = str, default = "localhost", help = "MQTT broker address")
    parser.add_argument("--port", type = int, default = 1883, help = "MQTT broker port")
    parser.add_argument("--topic", type = str, default = topic_suffix, help = "MQTT topic")
    parser.add_argument("--interval", type = int, default = 5, help = "Publishing interval in seconds")
    parser.add_argument("--randomness", type = float, default = 1, help = "Randomness added to the interval")
    parser.add_argument("--wallet", type = str, default = None, help = "Wallet address")
    args = parser.parse_args()

    sensor = sensor_class(args.id, args.broker, args.port, args.topic, args.wallet)
    try:
        sensor.run(args.interval, args.randomness)
    except KeyboardInterrupt:
        print(f"Stopping sensor {args.id}")
        sensor.disconnect()

if __name__ == "__main__":
    pass
