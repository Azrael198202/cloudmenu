import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber
export function getFoodInfo(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectShohinDetails.do',
    method: 'post',
    data: data
  })
}

export function searchKubun(data) {
  return request({
    url: '/order/searchKubun.do',
    method: 'post',
    data: data
  })
}
