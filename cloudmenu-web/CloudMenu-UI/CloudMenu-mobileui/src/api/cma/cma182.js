import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

export function searchNyukoProduct(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/searchNyukoProduct.do',
    method: 'post',
    data: data
  })
}

export function searchNyukoMaterial(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/searchNyukoMaterial.do',
    method: 'post',
    data: data
  })
}

export function searchNyukoEquipment(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/searchNyukoEquipment.do',
    method: 'post',
    data: data
  })
}
