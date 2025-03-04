const Mock = require('mockjs')
const { param2Obj } = require('./utils')

const user = require('./user')
const mockdemo = require('./mockdemo')
const role = require('./role')
const article = require('./article')
const common = require('./common')
const search = require('./remote-search')
const cma090 = require('./cma/cma090')
const cma080 = require('./cma/cma080')
const cma082 = require('./cma/cma082')
const cma100 = require('./cma/cma100')
const cma110 = require('./cma/cma110')
const cma120 = require('./cma/cma120')
const cma180 = require('./cma/cma180')
const cmb010 = require('./cmb/cmb010')
const cmb020 = require('./cmb/cmb020')
const cmc020 = require('./cmc/cmc020')
const cma181 = require('./cma/cma181')
const cma182 = require('./cma/cma182')
const cma183 = require('./cma/cma183')
const cma170 = require('./cma/cma170')
const cma200 = require('./cma/cma200')
const mocks = [
  ...user,
  ...mockdemo,
  ...role,
  ...article,
  ...common,
  ...search,
  ...cma090,
  ...cma080,
  ...cma100,
  ...cma110,
  ...cma120,
  ...cma180,
  ...cmb010,
  ...cmb020,
  ...cmc020,
  ...cma181,
  ...cma182,
  ...cma170,
  ...cma200,
  ...cma082,
  ...cma183
]

// for front mock
// please use it cautiously, it will redefine XMLHttpRequest,
// which will cause many of your third-party libraries to be invalidated(like progress event).
function mockXHR() {
  // mock patch
  // https://github.com/nuysoft/Mock/issues/300
  Mock.XHR.prototype.proxy_send = Mock.XHR.prototype.send
  Mock.XHR.prototype.send = function() {
    if (this.custom.xhr) {
      this.custom.xhr.withCredentials = this.withCredentials || false

      if (this.responseType) {
        this.custom.xhr.responseType = this.responseType
      }
    }
    this.proxy_send(...arguments)
  }

  function XHR2ExpressReqWrap(respond) {
    return function(options) {
      let result = null
      if (respond instanceof Function) {
        const { body, type, url } = options
        // https://expressjs.com/en/4x/api.html#req
        result = respond({
          method: type,
          body: JSON.parse(body),
          query: param2Obj(url)
        })
      } else {
        result = respond
      }
      return Mock.mock(result)
    }
  }

  for (const i of mocks) {
    Mock.mock(new RegExp(i.url), i.type || 'get', XHR2ExpressReqWrap(i.response))
  }
}

module.exports = {
  mocks,
  mockXHR
}
