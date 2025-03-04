import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber
export function getList() {
  const data = { storeNumber: storeNumber }
  return request({
    url: '/order/selectZasekiAllData.do',
    method: 'post',
    data: data
  })
}

export function insertReceptionInsideData(data) {
  return request({
    url: '/order/insertReceptionInsideData.do',
    method: 'post',
    data: data
  })
}

export function selectZasekiChoiceData(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectZasekiChoiceData.do',
    method: 'post',
    data: data
  })
}

export function searchKuBun(data) {
  return request({
    url: '/order/searchKubun',
    method: 'post',
    data: data
  })
}
