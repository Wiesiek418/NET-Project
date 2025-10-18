<template>
    <TheContainer>
        <template v-slot:main_content>
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
    </TheContainer>
    
</template>

<script>
import TableComponent from "../../components/TableComponent.vue";
import TheContainer from "../containers/TheContainer.vue";
import ChartView from "../../components/ChartView.vue";
import ApiService from '@/services/api';

export default {
    name: 'Home',
    components: {
        TheContainer,
        TableComponent,
        ChartView
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