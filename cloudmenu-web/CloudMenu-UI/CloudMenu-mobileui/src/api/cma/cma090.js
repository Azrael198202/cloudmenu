import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber
export function getList(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectShohinBunrui.do',
    method: 'post',
    data: data
  })
}
