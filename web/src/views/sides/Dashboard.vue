<template>
  <TheContainer>
    <template #main_content>
      <div class="dashboard-container">
          <h1>Dashboard</h1>
          <div class="dashboard-panel">
            <div
              class="dashboard-element"
              v-for="sensor in sensors"
            >
              <div v-if="sensor.values">
                <div class="dashboard-element-header">
                  <h2>{{ sensor.sensorId }} - {{ sensor.sensorType }}</h2>
                </div>
                <div class="dashboard-element-body">
                  <div class="dashboard-element-values">
                    <h3>Values:</h3>
                    <p>Last update: {{ parseDateTime(sensor.values[sensor.values.length - 1]?.Timestamp || sensor.values[sensor.values.length - 1]?.timestamp) }}</p>
                    <div
                      class="dashboard-element-avg"
                      v-for="(value, key) in sensor.avg"
                      :key="key"
                    >
                      {{ key }} {{ sensor.values[sensor.values.length - 1][key] }} AVG: {{ value }}
                    </div>
                  </div>
                  <div class="dashboard-charts">
                    <div 
                      class="dashboard-element-charts"
                      v-for="(value, key) in sensor.avg"
                      :key="value"
                    >
                      <h3>{{ key }}</h3>
                      <ChartView
                        v-if="sensor.values.length>0"
                        type="Bar"
                        :data="setChartData(sensor, key)"
                        :options="chartOptions"
                      >
                      </ChartView>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
      </div>
    </template>
  </TheContainer>
</template>

<script>
import TheContainer from "../containers/TheContainer.vue";
import WalletService from '@/services/wallet.js';
import CategoryService from '@/services/category.js';
import ChartView from "../../components/ChartView.vue";
import webSocket from "@/services/webSocket";
import dateMixin from "@/mixins/dateMixin";

export default {
  name: 'Dashboard',
  mixins: [ dateMixin ],
  components: {
    TheContainer,
    ChartView
  },
  data(){
    return {
        numberOfElements: 100,
        numberOfElementsOnChart: 10,
        sensors: {},
        dashboardValuesNames: {
            baking: [
                "gasFlow", "humidity", "temperature"
            ],
            packing: [
                "conveyorSpeed", "packageCount", "sealTemperature"
            ],
            dough: [
                "rotationSpeed", "motorTemperature", "vibrationLevel", "loadWeight"
            ],
            conveyor: [
                "bearingTemp", "speed"
            ],
        },
        convertCategory: ["baking", "packing", "dough", "conveyor"],
        sensorsValues: {},
        chartOptions: {
            responsive: true,
            maintainAspectRatio: false
        },
        unsubscribeSocket: null
    };
  },
  async mounted(){
    this.fetchData();
    await webSocket.connect("/notifications");
    this.unsubscribeSocket = webSocket.subscribe(this.websocketListener);
  },
  unmounted(){
    if(this.unsubscribeSocket){
      this.unsubscribeSocket();
    }
  },
  methods:{
    websocketListener(message){
      this.pushNewValue(message);
    },
    async fetchData(){
      const data = await WalletService.getWallets();
      this.sensors = data.filter(item =>
        !this.convertCategory.some(cat => 
          item.sensorType?.toLowerCase() === cat.toLowerCase()
        )
      ).map(item => {
          return {
              sensorId: item.sensorId,
              sensorType: this.convertCategory.find(cat => item.sensorType.toLowerCase().includes(cat)),
          };
      }).sort((a,b)=>a.sensorId > b.sensorId);
      for (const sensor of this.sensors) {
          const category = this.convertCategory.find(cat => sensor.sensorType.includes(cat));
          let sensorData = await CategoryService.getSensorAllData(sensor.sensorType, sensor.sensorId);
          if(!Array.isArray(sensorData)){
              sensorData = Object.values(sensorData);
          }
          sensor.sensorType = category;
          sensor.values = sensorData.slice(0,this.numberOfElements).reverse();
          sensor.avg = this.avgList(sensor);
      }
    },
    avgList(sensor, fixed = 2){
      if(sensor.values.length === 0) return 0;
      const categoryColumns = this.dashboardValuesNames[sensor.sensorType];
      let avgValues = {};
      for(const column of categoryColumns){
          const values = sensor.values.map(item => parseFloat(item[column])).filter(val => !isNaN(val));
          if(values.length === 0){
              avgValues[column] = 0;
          } else {
              avgValues[column] = (values.reduce((a, b) => a + b, 0).toFixed(fixed) / values.length).toFixed(fixed);
          }
      }
      return avgValues;
    },
    setChartData(sensor, key){
      const chunkSize = Math.ceil(sensor.values.length / this.numberOfElementsOnChart);
      const chunkData = this.getChunkData(sensor.values, key, chunkSize);
      return {
          labels: chunkData.map((_, index) => `#${index + 1}`),
          datasets: [
              {
                  label: key,
                  backgroundColor: '#f87979',
                  data: chunkData
              }
          ]
      };
    },
    getChunkData(values, key, chunkSize){
      let chunks = [];
      for (let i = 0; i < values.length; i += chunkSize) {
          const chunk = values.slice(i, i + chunkSize);
          const avg = (chunk.reduce((sum, item) => sum + parseFloat(item[key]), 0) / chunk.length).toFixed(2);
          chunks.push(avg);
      }
      return chunks;
    },
    pushNewValue(newValue){
      
      const sensor = this.sensors.find(s => s.sensorId === newValue.sensorId);
      if(sensor){
          sensor.values.push(newValue);
          if(sensor.values.length > this.numberOfElements){
              sensor.values.shift();
          }
          sensor.avg = this.avgList(sensor);
      }
    },
  }
}
</script>

<style>
.dashboard-element-body{
  display: flex;
  flex-direction: row;
  justify-content: align-start;
}
.dashboard-element-values{
  width: 300px;
  margin-right: 3rem;
}
.dashboard-charts{
  display: flex;
  flex-wrap: wrap;
  gap: 16px;
}
.dashboard-element-charts{
  flex: 1 1 auto;
  max-width: 400px;
  min-height: 250px;
  height: auto;
  margin-right: 2rem;
}

.dashboard-element-body h3{
  margin-top: 0;
}

</style>