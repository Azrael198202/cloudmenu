import request from '@/utils/request'
import defaultSettings from '@/settings'

const storeNumber = defaultSettings.storeNumber

export function searchNyukoList(data) {
    data.storeNumber = storeNumber
    return request({
        url: '/zaiko/searchNyukoList.do',
        method: 'post',
        data: data
    })
}

export function checkNyukoDate(data) {
    data.storeNumber = storeNumber
    return request({
        url: '/checkNyukoDate.do',
        method: 'post',
        data: data
    })
}