<template>
  <TheContainer>
    <template v-slot:main_content>
      <h1>Sensor View</h1>
      <p>This is the about page!</p>
      {{ sensorData  }}
    </template>
  </TheContainer>
</template>

<script>
import TheContainer from "../containers/TheContainer.vue";
import CategoryService from '@/services/category.js';

export default {
  name: 'SensorView',
  components: {
    TheContainer
  },
  data(){
    return {
        sensorData: null
    };
  },
  mounted(){
    this.fetchSensorData();
  },
  methods:{
    async fetchSensorData(){
        const sensorId = this.$route.params.id;
        const sensorCategory = this.$route.params.category;
        this.sensorData = await CategoryService.getSensorById(sensorCategory, sensorId);
    },
  }

};
</script>