import random
from base.sensor import BaseSensor, main

class ConveyorBeltSensor(BaseSensor):
    _STATUSES = ["active", "stopped", "misaligned", "ripped"]

    def _generate_data(self):
        return {
            "Status": random.choice(self._STATUSES),
            "BearingTemp": round(random.uniform(20.0, 80.0), 2),
            "Speed": round(random.uniform(0.5, 5.0), 2)
        }

if __name__ == "__main__":
    main(ConveyorBeltSensor, "sensors/conveyor")
