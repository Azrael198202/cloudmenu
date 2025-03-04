import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

export function selectOrderData(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectOrderData.do',
    method: 'post',
    data: data
  })
}

export function selectOrderHistoryData(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectOrderHistoryData.do',
    method: 'post',
    data: data
  })
}

export function cookingInstFlgUpdate(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/cookingInstFlgUpdate.do',
    method: 'post',
    data: data
  })
}

export function cookingInstSlipPrt(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/cookingInstSlipPrt.do',
    method: 'post',
    data: data
  })
}
