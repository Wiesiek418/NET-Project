<template>
  <div class="container">
    <HeaderComponent/>
    <div class="content-wrapper">
      <LeftSide :collapsed="sidebarCollapsed" @toggle="toggleSidebar"/>
      <main class="main-container">
        <div>
          <slot name="main_content"></slot>
        </div>
      </main>
    </div>
    <FooterComponent/>
  </div>
</template>

<script>
import FooterComponent from '../../components/main_page/FooterComponent.vue';
import HeaderComponent from '../../components/main_page/HeaderComponent.vue';
import LeftSide from '../../components/main_page/LeftSide.vue';

export default {
  name: 'TheContainer',
  components: {
    HeaderComponent,
    LeftSide,
    FooterComponent
  },
  data() {
    return {
      sidebarCollapsed: true
    };
  },
  methods: {
    toggleSidebar() {
      this.sidebarCollapsed = !this.sidebarCollapsed;
    },
  },
}
</script>

<style scoped>
.page-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.content-wrapper {
  display: flex;
  overflow: hidden;
  height: 100vh;
}

.main-content {
  flex: 1;
  padding: 1rem;
  overflow-y: auto;
}
</style>