import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

export function addNyukoProduct(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/addNyukoProduct.do',
    method: 'post',
    data: data
  })
}

export function addNyukoMaterial(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/addNyukoMaterial.do',
    method: 'post',
    data: data
  })
}

export function addNyukoEquipment(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/addNyukoEquipment.do',
    method: 'post',
    data: data
  })
}
