import { createWebHistory, createRouter } from 'vue-router'

import Home from '../views/sides/Home.vue'
import Blockchain from '../views/sides/Blockchain.vue'
import SensorView from '../views/sides/SensorView.vue'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home,
        alias: '/sensors'
    },
    {
        path: '/sensors/:category/:id',
        name: 'SensorsView',
        component: SensorView
    },
    {
        path: '/blockchain',
        name: 'Blockchain',
        component: Blockchain
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router