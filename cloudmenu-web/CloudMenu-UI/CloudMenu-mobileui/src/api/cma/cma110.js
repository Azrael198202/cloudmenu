import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber
export function insertOrderData(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/insertOrderData.do',
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
