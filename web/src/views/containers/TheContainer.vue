<template>
  <div class="container">
    <HeaderComponent/>
    <div class="content-wrapper">
      <LeftSide 
        :collapsed="uiStore.sidebarCollapsed" 
        @toggle="uiStore.toggleSidebar"
      />
      <main class="main-container">
        <slot name="main_content" />
      </main>
    </div>
    <FooterComponent/>
  </div>
</template>

<script>
import FooterComponent from '../../components/main_page/FooterComponent.vue'
import HeaderComponent from '../../components/main_page/HeaderComponent.vue'
import LeftSide from '../../components/main_page/LeftSide.vue'
import { useUiStore } from '@/stores/uiStore'

export default {
  name: 'TheContainer',
  components: { HeaderComponent, FooterComponent, LeftSide },
  setup() {
    const uiStore = useUiStore()
    return { uiStore }
  }
}
</script>

<style scoped>
.container {
  width: 100%;
  min-height: 100vh;
  margin: 0;
  padding: 0;
}

.content-wrapper {
  display: flex;
}

.main-container {
  flex: 1;
  padding: 1rem;
  margin: 1rem;
  overflow: visible;
  min-height: calc(100vh - 267px);
}
</style>
