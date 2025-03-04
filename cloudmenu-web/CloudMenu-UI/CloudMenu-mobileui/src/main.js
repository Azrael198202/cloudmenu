import Vue from 'vue'

import i18n from './langconfig/index'

import Cookies from 'js-cookie'

import 'normalize.css/normalize.css' // a modern alternative to CSS resets

import 'font-awesome/css/font-awesome.min.css'

import App from './App'
import store from './store'
import router from './router'

import './permission' // permission control
import './utils/error-log' // error log
import request from './utils/request' // error log

import storage from '@/utils/storage'

import businessMessage from '@/utils/message'

Vue.prototype.$msgUtil = businessMessage
Vue.prototype.$storage = storage

import Vant from 'vant'

import 'vant/lib/index.css'

import '@/styles/index.scss' // global css

import { validRemove } from '@/utils/validate'

import { formatAmount } from '@/utils/string'

Vue.use(Vant)
/**
 * If you don't want to use mock-server
 * you want to use MockJs for mock api
 * you can execute: mockXHR()
 *
 * Currently MockJs will be used in the production environment,
 * please remove it before going online ! ! !
 */
if (process.env.NODE_ENV === 'production') {
  const { mockXHR } = require('../mock')
  mockXHR()
}

Vue.config.productionTip = false
Vue.prototype.$request = request
// 删除动态追加的验证方法
Vue.prototype.validRemove = validRemove
Vue.prototype.formatAmount = formatAmount

new Vue({
  el: '#app',
  router,
  i18n,
  store,
  render: h => h(App)
})
