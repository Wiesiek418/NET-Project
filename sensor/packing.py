import random
from base.sensor import BaseSensor, main

class PackingLineSensor(BaseSensor):
    _STATUSES = ["running", "stopped", "maintenance", "error"]

    def __init__(self, sensor_id, broker_address, port, topic, wallet, auto_run):
        super().__init__(sensor_id, broker_address, port, topic, wallet, auto_run)
        self._package_count = 0
        self._error_count = 0

    def _generate_data(self):
        status = random.choice(self._STATUSES)
        if status == "running":
            self._package_count += random.randint(1, 10)
            if random.random() < 0.5:
                self._error_count += 1
        
        return {
            "ConveyorSpeed": round(random.uniform(0.5, 5.0), 2),
            "PackageCount": self._package_count,
            "SealTemperature": round(random.uniform(120.0, 200.0), 2),
            "ErrorCount": self._error_count,
            "Status": status
        }

if __name__ == "__main__":
    main(PackingLineSensor, "sensors/packing")
