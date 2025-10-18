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
    props: {
        collapsed: {
            type: Boolean,
            default: false
        }
    },
    data() {
        return {
            menuItems: [
                { name: 'Home', icon: '🏠', to: '/' },
                { name: 'About', icon: '👤', to: '/about' },
            ]
        };
    }
}
</script>

<style scoped>
.sidebar {
  transition: width 0.3s;
  background: #f5f5f5;
  padding: 1rem;
  box-shadow: 2px 0 5px rgba(0,0,0,0.1);
  overflow: hidden;
}

.sidebar.collapsed {
  width: 60px;
}

.toggle-btn {
  background: transparent;
  border: none;
  font-size: 1.2rem;
  cursor: pointer;
  margin-bottom: 1rem;
}

.menu {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.menu-item {
  display: flex;
  align-items: center;
  text-decoration: none;
  color: #333;
  padding: 0.5rem;
  border-radius: 0.3rem;
  transition: background-color 0.2s;
}

.menu-item:hover {
  background-color: #e0e0e0;
}

.menu-item .icon {
  width: 24px;
  text-align: center;
  margin-right: 0.5rem;
}

.menu-item.active {
  background-color: #1976d2;
  color: white;
}

.label {
  white-space: nowrap;
}
</style>