<template>
    <div class="sensors-container">
        <h3>Hello in {{ category }}</h3>
    </div>
</template>

<script>
import CategoryService from '@/services/category';

export default {
    name: 'SensorsView',
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
        };
    },
    mounted() {
        this.fetchData();
    },
    methods: {
        async fetchData() {
            this.sensorsData = await CategoryService.getCategory(this.category);
            console.log(this.sensorsData);
        },
    },
};
</script>