import random
from base.sensor import BaseSensor, main

class BakingFurnaceSensor(BaseSensor):
    _STATUSES = ["baking", "heating", "cooling", "error"]
    _DOOR_STATUSES = ["open", "closed"]

    def _generate_data(self):
        return {
            "Temperature": round(random.uniform(29.0, 250.0), 2),
            "Humidity": round(random.uniform(30.0, 50.0), 2),
            "GasFlow": round(random.uniform(10.0, 25.0), 2),
            "DoorStatus": random.choice(self._DOOR_STATUSES),
            "Status": random.choice(self._STATUSES)
        }

if __name__ == "__main__":
    main(BakingFurnaceSensor, "sensors/baking")
