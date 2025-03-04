import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

// 查询区分共同
export function searchKuBun(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/searchKubun.do',
    method: 'post',
    data: data
  })
}
