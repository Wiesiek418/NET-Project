
import argparse
import json
import random
import time
import paho.mqtt.client as mqtt
from datetime import datetime

class BaseSensor:
    def __init__(self, sensor_id, broker_address, port, topic):
        self.sensor_id = sensor_id
        self.broker_address = broker_address
        self.port = port
        self.topic = topic
        self.name = f"{topic}-sensor-{sensor_id}"
        self.client = mqtt.Client(client_id = self.name)
        self.client.on_connect = self.on_connect
        self.client.on_disconnect = self.on_disconnect

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

    def run(self, interval, randomness=0):
        self.connect()
        while True:
            data = self._generate_data()
            payload = {
                "SensorId": self.sensor_id,
                "Timestamp": datetime.now().isoformat(),
                **data
            }
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
    args = parser.parse_args()

    sensor = sensor_class(args.id, args.broker, args.port, args.topic)
    try:
        sensor.run(args.interval, args.randomness)
    except KeyboardInterrupt:
        print(f"Stopping sensor {args.id}")
        sensor.disconnect()

if __name__ == "__main__":
    pass
