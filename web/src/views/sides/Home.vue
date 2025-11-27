<template>
    <TheContainer>
        <template v-slot:main_content>
            <Tabs
                :tabs="tabs"
                @activeTab="activeTab"
            >
                <template #all>
                    <h1>Home View</h1>
                    <p>Welcome to the home page!</p>
                    <button @click="handleClick">Click Me</button>
                    <tableComponent
                        v-if="data"
                        :headers="headers"
                        :data="data">
                    </tableComponent>
                    <ChartView
                        v-if="chartData.labels && chartData.labels.length"
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
import ApiService from '@/services/api';
import Tabs from "../../components/Tabs.vue";
import SensorsView from "../SensorsView.vue";

export default {
    name: 'Home',
    components: {
        TheContainer,
        TableComponent,
        ChartView,
        Tabs,
        SensorsView,
    },
    data() {
        return {
            data: [],
            chartData: {
                labels: [],
                datasets: []
            },
            headers: {
                date: "Data",
                temperatureC: "Temp (°C)",
                temperatureF: "Temp (°F)",
                summary: "Opis" 
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
    methods:{
        async handleClick() {
            const value = await ApiService.get('/weatherforecast');
            this.data = value;
            this.chartData = this.prepareChartData();
        },
        prepareChartData() {
            if (!Array.isArray(this.data) || this.data.length === 0) {
                return { labels: [], datasets: [] }; // bezpieczny fallback
            }

            return {
                labels: this.data.map(item => item.date),
                datasets: [
                    {
                        label: 'Temperature (°C)',
                        backgroundColor: '#f87979',
                        data: this.data.map(item => item.temperatureC)
                    }
                ],
            };
        }
    }
};
</script>