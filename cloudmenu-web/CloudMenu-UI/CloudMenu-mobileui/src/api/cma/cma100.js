import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

// 根据参数判断行检索或者code检索
export function selectShohinCChoiceData(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectShohinCChoiceData.do',
    method: 'post',
    data: data
  })
}

// 根据参数判断行检索或者code检索
export function selectShohinBChoiceData(data) {
  data.storeNumber = storeNumber
  return request({
    url: '/order/selectShohinBChoiceData.do',
    method: 'post',
    data: data
  })
}

// 税率查询
export function serachCTaxKubun() {
  const data = {}
  data.storeNumber = storeNumber
  return request({
    url: '/order/serachCTaxKubun.do',
    method: 'post',
    data: data
  })
}
