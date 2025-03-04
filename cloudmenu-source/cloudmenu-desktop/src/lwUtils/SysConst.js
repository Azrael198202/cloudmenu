const isDevelopment = process.env.NODE_ENV !== 'production'
const baseUrl = process.env.VUE_APP_BASE_API

var storeNm = '老上海'
if (isDevelopment) {
  if (baseUrl.includes('localhost')) {
    storeNm = '老上海-開発版-LocalAPI'
  } else if (baseUrl.includes('lsh.leadingwin')) {
    storeNm = '老上海-開発版-TestSvAPI'
  } else if (baseUrl.includes('192.168.1.18')) {
    storeNm = '老上海-開発版-TestSvAPI'
  } else if (baseUrl.includes('cloudmenu.leadingwin.net')) {
    storeNm = '老上海-開発版-本番API'
  }
} else {
  const baseUrl = process.env.VUE_APP_BASE_API
  if (baseUrl.includes('localhost')) {
    storeNm = '老上海-本番-LocalAPI'
  } else if (baseUrl.includes('lsh.leadingwin')) {
    storeNm = '老上海-本番-TestSvAPI'
  } else if (baseUrl.includes('192.168.1.18')) {
    storeNm = '老上海-本番-TestSvAPI'
  } else if (baseUrl.includes('cloudmenu.leadingwin.net')) {
    storeNm = '老上海'
  }
}
const SysConst = {
  storeNumber: '000001',
  storeName: storeNm
  // storeNumber: '000002',
  // storeName: 'ラーメン屋さん'
  // storeNumber: '000003',
  // storeName: '居酒屋'
}

export default SysConst
