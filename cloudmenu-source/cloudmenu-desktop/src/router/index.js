import Vue from 'vue'
import VueRouter from 'vue-router'
import Menu from '../views/Menu.vue'
import Home from '../views/Home.vue'
import Cashiers from '../views/Cashiers.vue'
import MoneyInOut from '../views/MoneyInOut.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Menu',
    component: Menu
  },
  {
    path: '/home',
    name: 'Home',
    component: Home
  },
  {
    path: '/cashier/:recpnumber',
    // path: '/cashier',
    name: 'Cashiers',
    component: Cashiers
  },
  {
    path: '/moneyinout',
    name: 'MoneyInOut',
    component: MoneyInOut
  }
]

const router = new VueRouter({
  routes
})

export default router
