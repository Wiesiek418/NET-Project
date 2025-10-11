import { createMemoryHistory, createRouter } from 'vue-router'

import Home from '../views/sides/Home.vue'
import About from '../views/sides/About.vue'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/about',
        name: 'About',
        component: About
    }
]

const router = createRouter({
    history: createMemoryHistory(),
    routes
})

export default router