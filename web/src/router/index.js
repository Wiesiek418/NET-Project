import { createWebHistory, createRouter } from 'vue-router'

import Home from '../views/sides/Home.vue'
import About from '../views/sides/About.vue'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/blockchain',
        name: 'Blockchain',
        component: About
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router