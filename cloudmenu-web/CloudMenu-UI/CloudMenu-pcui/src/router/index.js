import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

import LayoutEmpty from '@/layoutempty'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    noCache: true                if set true, the page will no be cached(default is false)
    affix: true                  if set true, the tag will affix in the tags-view
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [{
        path: '/redirect',
        component: Layout,
        hidden: true,
        children: [{
            path: '/redirect/:path(.*)',
            component: () =>
                import ('@/views/redirect/index')
        }]
    },
    {
        path: '/login',
        component: () =>
            import ('@/views/demo/index'),
        hidden: true
    },
    {
        path: '/auth-redirect',
        component: () =>
            import ('@/views/login/auth-redirect'),
        hidden: true
    },
    {
        path: '/404',
        component: () =>
            import ('@/views/error-page/404'),
        hidden: true
    },
    {
        path: '/401',
        component: () =>
            import ('@/views/error-page/401'),
        hidden: true
    },
    {
        path: '/',
        component: Layout,
        redirect: '/dashboard',
        children: [{
            path: 'dashboard',
            component: () =>
                import ('@/views/dashboard/index'),
            name: 'Dashboard',
            meta: { title: 'Dashboard', icon: 'dashboard', affix: true }
        }]
    }
]

/**
 * asyncRoutes
 * the routes that need to be dynamically loaded based on user roles
 */
export const asyncRoutes = [{
        path: '/test',
        name: 'ErrorPages',
        component: Layout,
        children: [{
            path: '/demoPage',
            component: () =>
                import ('@/views/demo/index'),
            name: 'demoPage',
            meta: { title: 'demo', icon: 'demo', affix: true }
        }]
    },
    {
        path: 'external-link',
        component: Layout,
        children: [{
            path: 'https://github.com/PanJiaChen/vue-element-admin',
            meta: { title: 'External Link', icon: 'link' }
        }]
    },
    {
        path: '/cmaMenuList',
        name: 'cma010',
        component: LayoutEmpty,
        children: [{
            path: '/cmaMenuList',
            component: () =>
                import ('@/views/cma/cma010'),
            name: 'cma010',
            meta: { title: 'cma010', icon: 'cma010', affix: true }
        }]
    },
    {
        path: '/cmaMenuManage',
        component: () =>
            import ('@/views/cma/cma020')
    },
    {
        path: '/cmaGoodsList',
        name: 'cma030',
        component: LayoutEmpty,
        children: [{
            path: '/cmaGoodsList',
            component: () =>
                import ('@/views/cma/cma030'),
            name: 'cma030',
            meta: { title: 'cma030', icon: 'cma030', affix: true }
        }]
    },
    {
        path: 'cmaGoodsUp',
        component: LayoutEmpty,
        children: [{
            path: '/cmaGoodsUp',
            component: () =>
                import ('@/views/cma/cma040'),
            name: 'cma040',
            meta: { title: 'cma040', icon: 'cma040', affix: true }
        }]
    },
    {
        path: '/cmaTenpoInfoToroku',
        name: 'tenpoInfoToroku',
        component: LayoutEmpty,
        children: [{
            path: '/cmaTenpoInfoToroku',
            component: () =>
                import ('@/views/cma/cma050'),
            name: 'cma050',
            meta: { title: 'cma050', icon: 'cma050', affix: true }
        }]
    },
    {
        path: '/cmaTableToroku',
        name: 'tableToroku',
        component: LayoutEmpty,
        children: [{
            path: '/cmaTableToroku',
            component: () =>
                import ('@/views/cma/cma060'),
            name: 'cma060',
            meta: { title: 'cma060', icon: 'cma060', affix: true }
        }]
    },
    {
        path: '/cmaMaster',
        name: 'master',
        component: LayoutEmpty,
        children: [{
            path: '/cmaMaster',
            component: () =>
                import ('@/views/cma/cma070'),
            name: 'cma070',
            meta: { title: 'cma070', icon: 'cma070', affix: true }
        }]
    },
    {
        path: '/cmaTableUseInfoList',
        name: 'tableUseInfoList',
        component: LayoutEmpty,
        children: [{
            path: '/cmaTableUseInfoList',
            component: () =>
                import ('@/views/cma/cma140'),
            name: 'cma140',
            meta: { title: 'cma140', icon: 'cma140', affix: true }
        }]
    },
    {
        path: '/cmaKaikeiToroku',
        name: 'KaikeiToroku',
        component: LayoutEmpty,
        children: [{
            path: '/cmaKaikeiToroku',
            component: () =>
                import ('@/views/cma/cma150'),
            name: 'cma150',
            meta: { title: 'cma150', icon: 'cma150', affix: true }
        }]
    },
    {
        path: '/cmaZaikoItiran',
        name: 'zaikoItiran',
        component: LayoutEmpty,
        children: [{
            path: '/cmaZaikoItiran',
            component: () =>
                import ('@/views/cma/cma160'),
            name: 'cma160',
            meta: { title: 'cma160', icon: 'cma160', affix: true }
        }]
    },
    {
        path: '/cmaNyukoItiran',
        name: 'nyukoItiran',
        component: LayoutEmpty,
        children: [{
            path: '/cmaNyukoItiran',
            component: () =>
                import ('@/views/cma/cma170'),
            name: 'cma170',
            meta: { title: 'cma170', icon: 'cma170', affix: true }
        }]
    },
    {
        path: '/cmcStaffList',
        name: 'staffList',
        component: LayoutEmpty,
        children: [{
            path: '/cmcStaffList',
            component: () =>
                import ('@/views/cmc/cmc010'),
            name: 'cmc010',
            meta: { title: 'cmc010', icon: 'cmc010', affix: true }
        }]
    },

    // 404 page must be placed at the end !!!
    { path: '*', redirect: '/404', hidden: true }
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