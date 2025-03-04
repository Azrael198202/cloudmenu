import i18n from '@/langconfig/index'

/**
 * @param {string} path
 * @returns {Boolean}
 */
export function isExternal(path) {
  return /^(https?:|mailto:|tel:)/.test(path)
}

/**
 * @param {string} str
 * @returns {Boolean}
 */
export function validUsername(str) {
  const valid_map = ['admin', 'editor']
  return valid_map.indexOf(str.trim()) >= 0
}

/**
 * @param {string} url
 * @returns {Boolean}
 */
export function validURL(url) {
  const reg = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/
  return reg.test(url)
}

/**
 * @param {string} str
 * @returns {Boolean}
 */
export function validLowerCase(str) {
  const reg = /^[a-z]+$/
  return reg.test(str)
}

/**
 * @param {string} str
 * @returns {Boolean}
 */
export function validUpperCase(str) {
  const reg = /^[A-Z]+$/
  return reg.test(str)
}

/**
 * @param {string} str
 * @returns {Boolean}
 */
export function validAlphabets(str) {
  const reg = /^[A-Za-z]+$/
  return reg.test(str)
}

/**
 * @param {string} email
 * @returns {Boolean}
 */
export function validEmail(email) {
  const reg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
  return reg.test(email)
}

/**
 * @param {string} str
 * @returns {Boolean}
 */
export function isString(str) {
  if (typeof str === 'string' || str instanceof String) {
    return true
  }
  return false
}

/**
 * @param {Array} arg
 * @returns {Boolean}
 */
export function isArray(arg) {
  if (typeof Array.isArray === 'undefined') {
    return Object.prototype.toString.call(arg) === '[object Array]'
  }
  return Array.isArray(arg)
}

/**
 * @param {string} str
 * @returns {Boolean}
 * 必须含有英文和数字
 */
export function passwordCheck(password) {
  const reg = /^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]{6,20})$/
  return reg.test(password)
}

/* 只能输入英文和汉字*/
export function validEnglishOrKanji(value) {
  const reg = /^[A-Za-z\u4e00-\u9fa5]*$/
  return reg.test(value)
}

/* 只能输入英文和数字*/
export function validEnglishOrNum(rule, value, callback) {
  const reg = /^[A-Za-z0-9]+$/
  if (value == '' || value == undefined || value == null) {
    callback()
  } else {
    if (!reg.test(value) && value != '') {
      callback(new Error(i18n.t('message.E_0003')))
    } else {
      callback()
    }
  }
}

/* 只能输入英文和数字和下划线*/
export function validEnglishOrNumLine(rule, value, callback) {
  const reg = /^[A-Za-z0-9-_]+$/
  if (value == '' || value == undefined || value == null) {
    callback()
  } else {
    if (!reg.test(value) && value != '') {
      callback(new Error(i18n.t('message.E000003')))
    } else {
      callback()
    }
  }
}

/* 只能输入英文和数字和特殊符号*/
export function validEnglishOrNumsSymbol(rule, value, callback) {
  const reg = /^[a-z_A-Z0-9-\.!@#\$%\\\^&\*\)\(\+=\{\}\[\]\/",'<>~\·`\?:;|]+$/
  if (value == '' || value == undefined || value == null) {
    callback()
  } else {
    if (!reg.test(value) && value != '') {
      callback(new Error(i18n.t('message.E000004')))
    } else {
      callback()
    }
  }
}

/* 只能输入数字*/
export function validNum(rule, value, callback) {
  const reg = /^[0-9]*$/
  if (value == '' || value == undefined || value == null) {
    callback()
  } else {
    if (!reg.test(value) && value != '') {
      callback(new Error(i18n.t('message.E000005')))
    } else {
      callback()
    }
  }
}

/* 只能输入英文*/
export function validEnglish(rule, value, callback) {
  const reg = /^[A-Za-z]+$/
  if (value == '' || value == undefined || value == null) {
    callback()
  } else {
    if (!reg.test(value) && value != '') {
      callback(new Error(i18n.t('message.E000006')))
    } else {
      callback()
    }
  }
}

export function validatePassword(rule, value, callback) {
  if (value.length < 5) {
    callback(new Error(i18n.t('message.E000007')))
  } else {
    callback()
  }
}

export function validateServerError(rule, value, callback) {
  if (this.serverErrors) {
    this.serverErrors.forEach((v, i) => {
      if (v.msgItemId == rule.field) {
        callback(new Error(i18n.t('message.' + v.msgCode)))
        return
      }
    }, this)
  }
  callback()
}
