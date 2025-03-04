import router from './router'
import store from './store'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = [
  '/test',
  '/login',
  '/auth-redirect',
  '/agentRegister',
  '/demoPage',
  '/goods',
  '/dashboard',
  '/ProductMenu',
  '/foodInfo',
  '/Employee/seatSelection',
  '/Employee/ReceptionSelection',
  '/Employee/CommoditySearchMethod',
  '/Employee/CommodityOrder',
  '/Employee/OrderDecision',
  '/Employee/OrderHistory',
  '/Customer/CustomerMenu',
  '/Customer/ProductMenu',
  '/Customer/foodInfo',
  '/Employee/IncomingGoodsClassifySelect',
  '/Employee/inventoryInput',
  '/Employee/inventoryInput1',
  '/Employee/startOfWarehousing',
  '/Employee/SelectionOfIncomingoods',
  '/Employee/EmployeeMenu',
  '/Employee/EnterTheStockInQuantity',
  '/Employee/packageAcceptance',
  '/Employee/countChangeSel',
  '/Employee/changeOfOrderableQuantity'
] // no redirect whitelist

router.beforeEach(async (to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title
  document.title = getPageTitle(to.meta.title)
  // determine whether the user has logged in
  const hasToken = getToken()
  if (hasToken) {
    if (store.getters.getToken == null || store.getters.getToken === '') {
      store.commit('user/SET_TOKEN', hasToken)
      // var userInfo = getUserInfo()
      // store.commit('user/SET_NAME', userInfo.firstName)
    }

    if (to.path === '/login') {
      // if is logged in, redirect to the home page
      next({ path: '/' })
      NProgress.done() // hack: https://github.com/PanJiaChen/vue-element-admin/pull/2939
    } else {
      next()
    }
  } else {
    /* has no token*/
    if (whiteList.indexOf(to.path) !== -1) {
      next()
    } else {
      // other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
