import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber
export function searchStoreInfo() {
  const data = { storeNumber: storeNumber }
  return request({
    url: '/order/searchStoreInfo.do',
    method: 'post',
    data: data
  })
}
