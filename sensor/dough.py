import random
from base.sensor import BaseSensor, main

class DoughMixerSensor(BaseSensor):
    _STATUSES = ["mixing", "idle", "error"]

    def _generate_data(self):
        return {
            "RotationSpeed": round(random.uniform(30.0, 60.0), 2),
            "MotorTemperature": round(random.uniform(20.0, 90.0), 2),
            "VibrationLevel": round(random.uniform(0.1, 2.0), 2),
            "LoadWeight": round(random.uniform(10.0, 500.0), 2),
            "Status": random.choice(self._STATUSES)
        }

if __name__ == "__main__":
    main(DoughMixerSensor, "sensors/dough")
