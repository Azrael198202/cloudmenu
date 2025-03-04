import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

export function searchSystemMenu(data) {
    data.storeNumber = storeNumber
    return request({
        url: '/order/searchSystemMenu.do',
        method: 'post',
        data: data
    })
}