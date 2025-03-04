import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'
import Toasted from 'vue-toasted'
import { longClickDirective } from 'vue-long-click'
import lwHttp from './lwUtils/lwHttp'

Vue.config.productionTip = false

Vue.use(Toasted)
const longClickInstance = longClickDirective({ delay: 1000, interval: 0 })
Vue.directive('longclick', longClickInstance)
Vue.prototype.$lwHttp = lwHttp
new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
