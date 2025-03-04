import Cookies from 'js-cookie'
import store from '../store'

const TokenKey = 'Admin-Token'
const ShoppingKey = 'Shopping-Cart'
export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(token) {
  return Cookies.set(TokenKey, token)
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}

// 获取购物车
export function getShoppingCart() {
  var storeData = sessionStorage.getItem(ShoppingKey)
  if (storeData === undefined || storeData === '') {
    return { orderCount: 0, orderList: [] }
  }
  return JSON.parse(sessionStorage.getItem(ShoppingKey))
}

// 设置购物车,number作为key
export function setShoppingCart(food) {
  var orrderInfo = getShoppingCart()
  if (orrderInfo === undefined || orrderInfo === null) {
    orrderInfo = { orderCount: 1, orderList: [food] }
  } else {
    if (orrderInfo.orderList == null || orrderInfo.orderList === undefined) {
      orrderInfo.orderList = []
    }
    var hasFlag = false
    for (var i = 0; i < orrderInfo.orderList.length; i++) {
      if (food.number === orrderInfo.orderList[i].number) {
        orrderInfo.orderList[i] = food
        hasFlag = true
        break
      }
    }
    if (!hasFlag) {
      orrderInfo.orderList.push(food)
    }
    orrderInfo.orderCount = orrderInfo.orderList.length
  }
  store.commit('user/SET_ORDERCOUNT', orrderInfo.orderCount)
  return sessionStorage.setItem(ShoppingKey, JSON.stringify(orrderInfo))
}

// 删除购物车
export function removeShoppingCart(key) {
  var orrderInfo = getShoppingCart()
  for (var i = 0; i < orrderInfo.orderList.length; i++) {
    if (key === orrderInfo.orderList[i].number) {
      orrderInfo.orderList.splice(i, 1)
      break
    }
  }

  orrderInfo.orderCount = orrderInfo.orderList.length
  store.commit('user/SET_ORDERCOUNT', orrderInfo.orderCount)
  return sessionStorage.setItem(ShoppingKey, JSON.stringify(orrderInfo))
}

// 清空购物车所有信息
export function clearShoppingCart() {
  store.commit('user/SET_ORDERCOUNT', 0)
  sessionStorage.removeItem(ShoppingKey)
}
