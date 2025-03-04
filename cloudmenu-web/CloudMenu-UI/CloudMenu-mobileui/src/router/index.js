import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/redirect',
    component: Layout,
    hidden: true,
    children: [
      {
        path: '/redirect/:path(.*)',
        component: () => import('@/views/redirect/index')
      }
    ]
  },
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },

  {
    path: '/404',
    component: () => import('@/views/error-page/404'),
    hidden: true
  },
  {
    path: '/401',
    component: () => import('@/views/error-page/401'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [
      {
        path: 'dashboard',
        component: () => import('@/views/cmc/cmc020'),
        name: 'Dashboard',
        meta: { showFooter: true, showIndex: true, showLeft: false, showBack: false, showHeader: false }
      }
    ]
  },
  {
    path: '/',
    component: Layout,
    children: [
      {
        path: 'test',
        component: () => import('@/views/dashboard/index'),
        name: 'Dashboard',
        meta: { showFooter: true, showIndex: true, showLeft: false, showBack: false, showHeader: false }
      }
    ]
  }
]

/**
 * asyncRoutes
 * the routes that need to be dynamically loaded based on user roles
 */
export const asyncRoutes = [
  {
    path: '*',
    redirect: '/goods'
  },
  // showFooter是否显示tabBar
  // showIndex tabBar true:2个  false:4个
  // showLeft 控制头部左侧框框信息
  // showBack 控制头部返回箭头
  // showHeader 控制头部是否显示
  {
    path: '/Employee',
    component: Layout,
    children: [
      {
        path: 'EmployeeMenu',
        name: 'EmployeeMenu',
        component: () => import('@/views/cmc/cmc010'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: false, showHeader: true }
      },
      {
        path: 'ReceptionSelection',
        name: 'ReceptionSelection',
        component: () => import('@/views/cma/cma081'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: false, showHeader: true }
      },
      {
        path: 'SeatSelection',
        name: 'SeatSelection',
        component: () => import('@/views/cma/cma080'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: false, showHeader: true }
      },
      {
        path: 'packageAcceptance',
        name: 'packageAcceptance',
        component: () => import('@/views/cma/cma082'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: true, showHeader: true }
      },
      {
        path: 'CommoditySearchMethod',
        name: 'CommoditySearchMethod',
        component: () => import('@/views/cma/cma090'),
        meta: { showFooter: true, showIndex: false, showLeft: true, showBack: false, showHeader: true }
      },
      {
        path: 'CommodityOrder',
        name: 'CommodityOrder',
        component: () => import('@/views/cma/cma100'),
        meta: { showFooter: true, showIndex: false, showLeft: true, showBack: false, showHeader: true }
      },
      {
        path: 'OrderDecision',
        name: 'OrderDecision',
        component: () => import('@/views/cma/cma110'),
        meta: { showFooter: true, showIndex: false, showLeft: true, showBack: false, showHeader: true }
      },
      {
        path: 'OrderHistory',
        name: 'OrderHistory',
        component: () => import('@/views/cma/cma120'),
        meta: { showFooter: true, showIndex: false, showLeft: true, showBack: false, showHeader: true }
      },
      {
        path: 'startOfWarehousing',
        name: 'startOfWarehousing',
        component: () => import('@/views/cma/cma180'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: false, showHeader: true }
      },
      {
        path: 'inventoryInput',
        name: 'inventoryInput',
        component: () => import('@/views/cma/cma170'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: false, showHeader: true }
      },
      {
        path: 'IncomingGoodsClassifySelect',
        name: 'IncomingGoodsClassifySelect',
        component: () => import('@/views/cma/cma181'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: true, showHeader: true }
      },
      {
        path: 'SelectionOfIncomingoods',
        name: 'SelectionOfIncomingoods',
        component: () => import('@/views/cma/cma182'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: true, showHeader: true }
      },
      {
        path: 'EnterTheStockInQuantity',
        name: 'EnterTheStockInQuantity',
        component: () => import('@/views/cma/cma183'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: true, showHeader: true }
      },
      {
        path: 'countChangeSel',
        name: 'countChangeSel',
        component: () => import('@/views/cma/cma190'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: false, showHeader: true }
      },
      {
        path: 'changeOfOrderableQuantity',
        name: 'changeOfOrderableQuantity',
        component: () => import('@/views/cma/cma200'),
        meta: { showFooter: false, showIndex: false, showLeft: false, showBack: true, showHeader: true }
      }
    ]
  },
  {
    path: '/Customer',
    component: Layout,
    children: [
      {
        path: 'CustomerMenu',
        component: () => import('@/views/cmc/cmc020'),
        name: 'CustomerMenu',
        meta: { showFooter: true, showIndex: true, showLeft: false, showBack: false, showHeader: false }
      },
      {
        path: 'ProductMenu',
        name: 'ProductMenu',
        component: () => import('@/views/cmb/cmb010'),
        meta: { showFooter: true, showIndex: true, showLeft: false, showBack: false, showHeader: true }
      },
      {
        path: 'foodInfo',
        name: 'foodInfo',
        component: () => import('@/views/cmb/cmb020'),
        meta: { showFooter: true, showIndex: true, showLeft: false, showBack: true, showHeader: true }
      }
    ]
  }
]

const createRouter = () =>
  new Router({
    mode: 'history', // require service support
    scrollBehavior: () => ({ y: 0 }),
    routes: constantRoutes.concat(asyncRoutes)
  })

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
