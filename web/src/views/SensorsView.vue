<template>
    <div class="sensors-container">
        <div class="download-btn-container">
            <button
                @click="downloadData('json')"
                class="download-btn"
            >
                Download Data JSON
            </button>
            <button
                @click="downloadData('csv')"
                class="download-btn"
            >
                Download Data CSV
            </button>
        </div>
        <FilterSortPanel
            :headers="headers[category]"
            @apply="fetchData"
        />
        <p>Total rows: <strong>{{ sensorsData.length }}</strong></p>
        <tableComponent
            v-if="sensorsData"
            :headers="headers[category]"
            :data="sensorsData"
            @rowClick="handleRowClick"
        >
        </tableComponent>
    </div>
</template>

<script>
import CategoryService from '@/services/category';
import TableComponent from "@/components/TableComponent.vue";
import FilterSortPanel from "@/components/FilterSortPanel.vue";
import dateMixin from '@/mixins/dateMixin';

export default {
    name: 'SensorsView',
    mixins: [dateMixin],
    components: {
        TableComponent,
        FilterSortPanel,
    },
    props: {
        category: {
            type: String,
            required: false,
            default: 'all',
        },
    },
    data() {
        return {
            sensorsData: [],
            headers: {
                baking:{
                    sensorId: "Sensor ID",
                    temperature: "Temperature (°C)",
                    humidity: "Humidity (%)",
                    gasFlow: "Gas Flow (m3/h)",
                    doorStatus: "Door Status",
                    timestamp: "Timestamp",
                    status: "Status",
                },
                conveyor:{
                    sensorId: "Sensor ID",
                    bearingTemp: "Bearing Temperature (°C)",
                    speed: "Speed (m/s)",
                    timestamp: "Timestamp",
                    status: "Status",
                },
                dough:{
                    sensorId: "Sensor ID",
                    motorTemperature: "Motor Temperature (°C)",
                    loadWeight: "Load Weight (kg)",
                    rotationSpeed: "Rotation Speed (rpm)",
                    vibrationLevel: "Vibration Level (mm/s)",
                    timestamp: "Timestamp",
                    status: "Status",
                },
                packing:{
                    sensorId: "Sensor ID",
                    sealTemperature: "Seal Temperature (°C)",
                    conveyorSpeed: "Conveyor Speed (m/s)",
                    packageCount: "Package Count",
                    errorCount: "Error Count",
                    timestamp: "Timestamp",
                    status: "Status",
                },
                    
            },
            filters: {},
        };
    },
    mounted() {
        this.fetchData();
    },
    methods: {
        async fetchData(filters={}) {
            this.filters = filters;
            this.sensorsData = await CategoryService.getCategory(this.category, this.filters.filters, this.filters.sort);
        },
        async downloadData(format) {
            await CategoryService.downloadCategory(this.category, this.filters.filters, this.filters.sort, `${this.category}_sensors_data`,format);
        },
        handleRowClick(row){
            this.$router.push(`/sensors/${this.category}/${row.id}`);
        }
    },
};
</script>
<style>
.download-btn-container{
    margin: 5px 0;
}
.download-btn{
    margin-right: 4px;
    background-color: var(--theme-color-thirdly);
    font-weight: bold;
}
</style>