<template>
  <TheContainer>
    <template v-slot:main_content>
      <h1>Blockchain View</h1>
      <p>This is the about page!</p>
    </template>
  </TheContainer>
</template>

<script>
import TheContainer from "../containers/TheContainer.vue";

export default {
  name: 'Blockchain',
  components: {
    TheContainer
  }
};
</script>