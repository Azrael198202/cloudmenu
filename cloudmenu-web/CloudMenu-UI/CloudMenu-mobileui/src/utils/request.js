import axios from 'axios'
import { Notify } from 'vant'
import store from '@/store'
import { getToken } from '@/utils/auth'

import businessMessage from '@/utils/message'

// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 50000 // request timeout
})

// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent
    if (store.getters.token) {
      config.headers['Authorization'] = getToken() // 让每个请求携带自定义token 请根据实际情况自行修改
    }
    return config
  },

  error => {
    // do something with request error
    console.log(error) // for debug
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
   */

  /**
   * Determine the request status by custom code
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    const res = response.data

    // if the custom code is not 20000, it is judged as an error.
    if (
      res.status !== 200 &&
      res.status !== 601 &&
      res.status !== 602 &&
      res.status !== 404 &&
      res.status !== 400 &&
      res.status !== 423
    ) {
      Notify({
        message: res.msgContent || 'Error',
        type: 'error',
        duration: 5 * 1000
      })

      // 50008: Illegal token; 50012: Other clients logged in; 50014: Token expired;
      if (res.status === 500) {
        businessMessage.messageBox('系统发生错误，请联系管理员反馈问题。')
      }
      return Promise.reject(new Error(res.message || 'Error'))
    } else {
      if (res.status === 601 || res.status === 602) {
        var msgCode = res.msgList.msgCode
        var msgInfo = res.msgList.msgOption
        res.msgCode = msgCode
        res.msgInfo = msgInfo
      }
      return res
    }
  },
  error => {
    console.log('err' + error) // for debug
    Notify({
      message: error.message,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
