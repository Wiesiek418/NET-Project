<template>
    <div class="sensors-container">
        <button
            @click="downloadData('json')"
        >
            Download Data JSON
        </button>
        <button
            @click="downloadData('csv')"
        >
            Download Data CSV
        </button>

        <FilterSortPanel
            :headers="headers[category]"
            @apply="fetchData"
        />
    
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

export default {
    name: 'SensorsView',
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
                    SensorId: "Sensor ID",
                    Temperature: "Temperature (°C)",
                    Humidity: "Humidity (%)",
                    GasFlow: "Gas Flow (m3/h)",
                    DoorStatus: "Door Status",
                    Timestamp: "Timestamp",
                    Status: "Status",
                },
                conveyor:{
                    SensorId: "Sensor ID",
                    BearingTemp: "Bearing Temperature (°C)",
                    Speed: "Speed (m/s)",
                    Timestamp: "Timestamp",
                    Status: "Status",
                },
                dough:{
                    SensorId: "Sensor ID",
                    MotorTemperature: "Motor Temperature (°C)",
                    LoadWeight: "Load Weight (kg)",
                    RotationSpeed: "Rotation Speed (rpm)",
                    VibrationLevel: "Vibration Level (mm/s)",
                    Timestamp: "Timestamp",
                    Status: "Status",
                },
                packing:{
                    SensorId: "Sensor ID",
                    SealTemperature: "Seal Temperature (°C)",
                    ConveyorSpeed: "Conveyor Speed (m/s)",
                    PackageCount: "Package Count",
                    ErrorCount: "Error Count",
                    Timestamp: "Timestamp",
                    Status: "Status",
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
            this.$router.push(`/sensors/${this.category}/${row.Id}`);
        }
    },
};
</script>