import { defineStore } from "pinia";

export const useSensorsStore = defineStore("sensors", {
    state: () => ({
        sensors: {},
    }),
    actions: {
        setSensorsList(sensorsList) {
            sensorsList.forEach(sensor => {
                this.sensors[sensor.sensorId] = { 
                    sensorId: sensor.sensorId,
                    sensorType: sensor.sensorType,
                    values: []
                };
            });
        },
        getSensorsData()
        {
            return this.sensors;
        },
        setInitialSensorValues(sensorId, values) {
            if (this.sensors[sensorId]) {
                this.sensors[sensorId] = values.slice(10);
            }
        },
        pushValueToSensor(sensorId, value) {
            const arr = this.sensors[sensorId]?.values;
            if (arr) {
                arr.push(value);
                if (arr.length > 10) {
                    arr.shift();
                }
            }
        }
    },
});
