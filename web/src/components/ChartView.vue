<template>
    <div class="chart-container">
        <div class="chart-card">
            <component
                :is="chartComponent"
                :data="data"
                :options="options"
            />
        </div>
    </div>
</template>

<script>
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  ArcElement,
  BarElement,
  CategoryScale,
  LinearScale
} from "chart.js";
import { Bar, Pie } from "vue-chartjs";

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  ArcElement,
  BarElement,
  CategoryScale,
  LinearScale
);

export default {
    name: 'ChartView',
    components: { Bar, Pie },
    props: {
        data: {
            type: Object,
            required: true
        },
        options: {
            type: Object,
            required: false,
            default: () => ({responisve: true})
        },
        type: {
            type: String,
            required: false,
            default: 'bar',
            validator: value => ['bar', 'pie'].includes(value)
        }
    },
    computed: {
        chartComponent() {
            return this.type === 'pie' ? 'Pie' : 'Bar';
        },
        chartData() {
            return this.data;
        },
        chartOptions() {
            return this.options;
        }
    }
};
</script>

<style scoped>
.chart-container {
  width: 100%;
  max-width: 800px;
  margin: 2rem auto;
}

.chart-card {
  background: white;
  border-radius: 1rem;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}
</style>