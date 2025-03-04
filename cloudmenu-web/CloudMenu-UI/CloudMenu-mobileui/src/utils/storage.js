import request from '@/utils/request' // request

export const local = {
  set(key, value) {
    localStorage.setItem(key, JSON.stringify(value))
  },
  get(key) {
    return JSON.parse(localStorage.getItem(key))
  },
  remove(key) {
    localStorage.removeItem(key)
  }
}
export const session = {
  set(key, value) {
    sessionStorage.setItem(key, JSON.stringify(value))
  },
  get(key) {
    return JSON.parse(sessionStorage.getItem(key))
  },
  remove(key) {
    sessionStorage.removeItem(key)
  }
}

/**
 * get Categorys form WebStorage
 * @author Qu
 * @param key categoryid
 * @returns categorys
 */
async function getCategorys(key) {
  var value = sessionStorage.getItem(key)
  if (typeof value == 'undefined' || value == null) {
    var response = await request({
      url: '/category/getAllCategorys.do',
      headers: { 'content-type': 'application/json' },
      method: 'post',
      params: {}
    })
    if (response.status == 200) {
      var codeList = new Array()
      var parentCode = null
      var resultData = response.data
      resultData.sort(function(a, b) {
        var x = a.categoryParentCode.toLowerCase()
        var y = b.categoryParentCode.toLowerCase()
        if (x < y) {
          return -1
        }
        if (x > y) {
          return 1
        }
        return 0
      })
      resultData.forEach(element => {
        if (parentCode == null) {
          parentCode = element.categoryParentCode
          codeList.push(element)
        } else if (parentCode == element.categoryParentCode) {
          codeList.push(element)
        } else {
          sessionStorage.setItem(parentCode, JSON.stringify(codeList))
          parentCode = element.categoryParentCode
          codeList = new Array()
          codeList.push(element)
          resultData.indexOf(element)
        }
        if (resultData.indexOf(element) + 1 == resultData.length) {
          sessionStorage.setItem(parentCode, JSON.stringify(codeList))
          parentCode = null
          codeList = new Array()
        }
      })
    }
    return JSON.parse(sessionStorage.getItem(key))
  } else {
    return JSON.parse(value)
  }
}

export default getCategorys
