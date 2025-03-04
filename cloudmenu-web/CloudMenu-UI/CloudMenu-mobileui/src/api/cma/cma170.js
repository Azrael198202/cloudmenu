import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

export function searchTana(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/zaiko/searchTana.do',
    method: 'post',
    data: data
  })
}

export function saveTana(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/zaiko/saveTana.do',
    method: 'post',
    data: data
  })
}
