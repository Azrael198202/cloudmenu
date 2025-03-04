import request from '@/utils/request'

export function getList(data) {
  return request({
    url: '/order/searchKubun.do',
    method: 'post',
    data: data
  })
}
