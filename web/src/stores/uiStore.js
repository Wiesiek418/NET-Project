import { defineStore } from 'pinia';

export const useUiStore = defineStore('ui', {
  state: () => ({
    sidebarCollapsed: true,
  }),
  actions: {
    toggleSidebar() {
      this.sidebarCollapsed = !this.sidebarCollapsed;
    },
  },
});
