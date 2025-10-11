<template>
    <TheContainer>
        <template v-slot:main_content>
        <h1>Home View</h1>
        <p>Welcome to the home page!</p>
        </template>
    </TheContainer>
    <button @click="handleClick">Click Me</button>
    <div v-if="data">
        <h2>Weather Forecast:</h2>
        <ul>
            <li v-for="(item, index) in data" :key="index">
                {{ item.date }} - {{ item.temperatureC }}°C - {{ item.summary }}
            </li>
       </ul>
    </div>  
</template>

<script>
import TheContainer from "../containers/TheContainer.vue";
import ApiService from '@/services/api';

export default {
    name: 'Home',
    components: {
        TheContainer
    },
    data() {
        return {
            data: {}
        };
    },
    methods:{
        async handleClick() {
            const value = await ApiService.get('/weatherforecast');
            this.data = value;
        }
    }
};
</script>