import { MessageBox } from 'element-ui'
import { Message } from 'element-ui'
import i18n from '@/langconfig/index'

function BusinessMessage() {
  this.init
  this.messages
}
BusinessMessage.prototype = {
  get: function(id, args) {
    var msgPparam = []
    if (args) {
      if (typeof args == 'object' && args.length) {
        for (var i = 0; i < args.length; i++) {
          msgPparam.push(i18n.t(args[i]))
        }
      } else {
        msgPparam.push(i18n.t(args))
      }
    }

    var message = i18n.t('message.' + id, msgPparam)
    if (!message && message !== '') {
      return id
    }
    return message
  },
  message: function(id, args) {
    if (id.indexOf('E') == 0) {
      return Message({
        message: this.get(id, args),
        type: 'error'
      })
    } else if (id.indexOf('W') == 0) {
      return Message({
        message: this.get(id, args),
        type: 'warning'
      })
    } else if (id.indexOf('I') == 0) {
      return Message({
        message: this.get(id, args),
        type: 'success'
      })
    } else {
      return Message({
        message: this.get(id, args),
        type: 'info'
      })
    }
  },
  messageBox: function(id, args) {
    if (id.indexOf('E') == 0) {
      return MessageBox.alert(this.get(id, args), i18n.t('message.messageErrorTitle'), {
        confirmButtonText: i18n.t('message.messageOkButton'),
        confirmButtonClass: 'el-button--mini',
        type: 'error'
      })
    } else if (id.indexOf('W') == 0) {
      return MessageBox.confirm(this.get(id, args), i18n.t('message.messageConfirmTitle'), {
        confirmButtonText: i18n.t('message.messageOkButton'),
        cancelButtonText: i18n.t('message.messageCancleButton'),
        cancelButtonClass: 'el-button--mini white-button',
        confirmButtonClass: 'el-button--mini',
        type: 'warning'
      })
    } else if (id.indexOf('I') == 0) {
      return MessageBox.alert(this.get(id, args), i18n.t('message.messageInfoTitle'), {
        confirmButtonText: i18n.t('message.messageOkButton'),
        confirmButtonClass: 'el-button--mini',
        type: 'success'
      })
    } else {
      return MessageBox.alert(this.get(id, args), i18n.t('message.messageInfoTitle'), {
        confirmButtonText: i18n.t('message.messageOkButton'),
        confirmButtonClass: 'el-button--mini',
        type: 'info'
      })
    }
  }
}

export default new BusinessMessage()
