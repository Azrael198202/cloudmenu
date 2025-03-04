import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

// 查询区分共同
export function selectShohinTypeKbnData(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectShohinTypeKbnData.do',
    method: 'post',
    data: data
  })
}

// 注文可能数変更
export function updateLimitedQuantity(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/updateLimitedQuantity.do',
    method: 'post',
    data: data
  })
}
