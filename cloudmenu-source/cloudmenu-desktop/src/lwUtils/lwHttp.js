const lwHttp = {
  // baseUrl: isDevelopment ? 'http://localhost:5000/api/' : 'https://cloudmenu.leadingwin.net/api/',
  baseUrl: process.env.VUE_APP_BASE_API,

  /**
   * Api呼び出す共通メソッド
   * @param {*} apiUrl
   * @param {*} data
   * @returns
   */
  postAsync: async function (apiUrl = '', data = {}) {
    // Default options are marked with *
    const fullUrl = this.baseUrl + apiUrl

    const response = await fetch(fullUrl, {
      method: 'POST', // *GET, POST, PUT, DELETE, etc.
      mode: 'cors', // no-cors, *cors, same-origin
      cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
      credentials: 'omit', // include, *same-origin, omit
      headers: {
        'Content-Type': 'application/json'
        // 'Content-Type': 'application/x-www-form-urlencoded',
      },
      redirect: 'follow', // manual, *follow, error
      referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
      body: JSON.stringify(data) // body data type must match "Content-Type" header
    })
    return response.json() // parses JSON response into native JavaScript objects
  },
  /**
   * EPSONプリンター印刷処理呼び出す
   */
  printChit: function (printerAddrs = '', xmlData = '') {
    return new Promise(function (resolve, reject) {
      var xhr = new XMLHttpRequest()
      xhr.open('POST', 'http://' + printerAddrs, true)
      xhr.timeout = 5000
      xhr.setRequestHeader('Content-Type', 'text/xml; charset=utf-8')
      xhr.setRequestHeader('If-Modified-Since', 'Thu, 01 Jan 1970 00:00:00 GMT')
      xhr.setRequestHeader('SOAPAction', '""')
      xhr.onreadystatechange = function () {
        // Receive response document
        if (xhr.readyState === 4) {
          // Parse response document
          if (xhr.status === 200) {
            const success = xhr.responseXML.getElementsByTagName('response')[0].getAttribute('success')
            const msgCode = xhr.responseXML.getElementsByTagName('response')[0].getAttribute('code')
            //* EPSONプリンタエラーメッセージ
            const codeMsgs = {
              EPTR_AUTOMATICAL: '連続印刷でエラーが発生しました。',
              EPTR_COVER_OPEN: 'カバーが開いています。',
              EPTR_CUTTER: 'カッターに異物があります。',
              EPTR_MECHANICAL: '機械的エラー',
              EPTR_REC_EMPTY: '印刷用紙がない。',
              EPTR_UNRECOVERABLE: '低い電圧',
              EX_BADPORT: 'プリンターが接続されていません。',
              EX_TIMEOUT: '印刷タイムアウト'
            }
            let message = codeMsgs[msgCode]
            if (!message) {
              message = ''
            }
            resolve({
              success: success,
              code: msgCode,
              message: message
            })
          } else {
            resolve({
              success: 'false',
              code: 'NETWORK_ERR',
              message: 'ネットワークエラーが発生しました。'
            })
          }
        }
      }
      xhr.send(xmlData)
    })
  }
}

export default lwHttp
