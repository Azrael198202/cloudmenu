import Vue from 'vue'
import Vuex from 'vuex'
import getters from './getters'

Vue.use(Vuex)

// https://webpack.js.org/guides/dependency-management/#requirecontext
const modulesFiles = require.context('./modules', true, /\.js$/)

// you do not need `import app from './modules/app'`
// it will auto require all vuex module from modules file
const modules = modulesFiles.keys().reduce((modules, modulePath) => {
  // set './app.js' => 'app'
  const moduleName = modulePath.replace(/^\.\/(.*)\.\w+$/, '$1')
  const value = modulesFiles(modulePath)
  modules[moduleName] = value.default
  return modules
}, {})

const store = new Vuex.Store({
  modules,
  getters,
  state: { // 设置属性 用来存储数据
    setScrollY: 0 // 页面滚动条显示高度
  },
  mutations: { // 更改属性的状态
    setScrollYCommit(state, data) {
      state.setScrollY = data
    }
  },
  actions: { // 应用 mutation
    setScrollYAction({ commit }, data) {
      commit('setScrollYCommit', data)
    }
  }
})

export default store
