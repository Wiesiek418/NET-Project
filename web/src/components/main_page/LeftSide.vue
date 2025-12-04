<template>
    <aside :class="['sidebar', { collapsed }]">
        <button class="toggle-btn" @click="$emit('toggle')">
            {{ collapsed ? 'Expand' : 'Collapse' }}
        </button>

        <nav class="left-aside-menu">
            <router-link
                v-for="item in menuItems" 
                :key="item.name"
                :to="item.to"
                class="menu-item"
                active-class="active"
            >
                <span class="icon">{{ item.icon }}</span>
                <span v-if="!collapsed" class="label">{{ item.name }}</span>
            </router-link>
        </nav>
    </aside>
</template>

<script>
export default {
    name: 'LeftSide',
    emits: ['toggle'],
    props: {
        collapsed: {
            type: Boolean,
            default: false
        }
    },
    data() {
        return {
            menuItems: [
                { name: 'Sensors', icon: '🏠', to: '/' },
                { name: 'Blockchain', icon: '👤', to: '/blockchain' },
                { name: 'Dashboard', icon: '💻', to: '/dashboard' },
            ]
        };
    }
}
</script>

<style scoped>
.sidebar {
  width: 220px;
  min-width: 100px;
  transition: width 0.3s ease;
  background: var(--theme-color-secondary);
  color: var(--theme-color-font-secondary);
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-items: stretch;
}

.sidebar.collapsed {
  width: 80px;
}

.toggle-btn {
  background: transparent;
  color: var(--theme-color-primary);
  font-weight: var(--font-weight-bold);
  border: none;
  font-size: 1.2rem;
  cursor: pointer;
  margin-bottom: 1rem auto;
  display: flex;
  align-items: center;
  justify-content: center;
}

.left-aside-menu {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  flex-grow: 1;
}

.menu-item {
  display: flex;
  align-items: center;
  text-decoration: none;
  color: #333;
  padding: 0.75rem 1rem;
  border-radius: 0.5rem;
  transition: all 0.25s ease;
  font-size: 1rem;
}

.menu-item:hover {
  background-color: #e0e0e0;
}

.icon {
  font-size: 1.8rem;
  width: 32px;
  text-align: center;
  transition: transform 0.3s ease, margin 0.3s ease;
}

.label{
  margin-left: 1.5rem;
  font-weight: bold;
}

.sidebar.collapsed .menu-item {
  justify-content: center;
}

.menu-item.active {
  background-color: var(--theme-color-thirdly);
  color: white;
  box-shadow: inset 3px 0 0 var(--theme-color-thirdly-shadow);
}

.menu-item.active:hover {
  background-color: var(--theme-color-thirdly-hover);
}

</style>