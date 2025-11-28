<template>
    <TheContainer>
        <template v-slot:main_content>
            <Tabs
                :tabs="tabs"
                @activeTab="activeTab"
            >
                <template #all>
                    <div class="home-sensors-container">
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
                            :headers="headers"
                            @apply="fetchData"
                        />
                    
                        <tableComponent
                            v-if="sensorsListData"
                            :headers="headers"
                            :data="sensorsListData"
                        >
                        </tableComponent>
                    </div>
                    <ChartView
                        v-if="false"
                        type="bar"
                        :data="chartData"
                    /> 
                </template>
                <template
                    v-for="category in categories"
                    #[category]
                >
                    <SensorsView
                        :category="category"
                    />
                </template>
            </Tabs> 
        </template>
    </TheContainer>
</template>

<script>
import TableComponent from "../../components/TableComponent.vue";
import TheContainer from "../containers/TheContainer.vue";
import ChartView from "../../components/ChartView.vue";
import Tabs from "../../components/Tabs.vue";
import SensorsView from "../SensorsView.vue";
import CategoryService from '@/services/category';
import FilterSortPanel from "@/components/FilterSortPanel.vue";

export default {
    name: 'Home',
    components: {
        TheContainer,
        TableComponent,
        ChartView,
        Tabs,
        SensorsView,
        FilterSortPanel,
    },
    data() {
        return {
            sensorsListData: [],
            chartData: {
                labels: [],
                datasets: []
            },
            headers: {
                SensorId: "Sensor ID",
                Type: "Category",
                CreatedAt: "Created at",
            },
            tabs:[
                { key: 'all', label: 'All' },
                { key: 'baking', label: 'Baking' },
                { key: 'conveyor', label: 'Conveyor' },
                { key: 'dough', label: 'Dough' },
                { key: 'packing', label: 'Packing' },
            ],
            categories:[
                'baking',
                'conveyor',
                'dough' ,
                'packing',
            ],
            activeTab: 'all',
        };
    },
    mounted(){
        this.fetchData();
    },
    methods:{
        // async handleClick() {
        //     const value = await ApiService.get('/weatherforecast');
        //     this.data = value;
        //     this.chartData = this.prepareChartData();
        // },
        // prepareChartData() {
        //     if (!Array.isArray(this.data) || this.data.length === 0) {
        //         return { labels: [], datasets: [] }; // bezpieczny fallback
        //     }

        //     return {
        //         labels: this.data.map(item => item.date),
        //         datasets: [
        //             {
        //                 label: 'Temperature (°C)',
        //                 backgroundColor: '#f87979',
        //                 data: this.data.map(item => item.temperatureC)
        //             }
        //         ],
        //     };
        // },
        async fetchData(filters={}) {
            this.filters = filters;
            this.sensorsListData = await CategoryService.getSensors(this.filters.filters, this.filters.sort);
        },
        async downloadData(format) {
            await CategoryService.downloadSensors(this.filters.filters, this.filters.sort, `sensors_list_data`,format);
        }
    }
};
</script>