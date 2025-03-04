import { Notify } from 'vant'
import { Dialog } from 'vant'

function BusinessMessage() {
  this.init
  this.messages
}
BusinessMessage.prototype = {
  get: function(id, args) {
    if (!this.init) {
      this.messages = []
      this.messages['I_00050'] = '確定していない入力はクリアされます。よろしいですか？'
      this.messages['E_00010'] = '{0}検索でエラーが発生しました。'
      this.messages['I_00030'] = '注文を確定しました({0})。'
      this.messages['E_00020'] = '{0}を入力してください。'
      this.messages['E_00030'] = '準備中のため選択できません。'
      this.messages['W_00010'] = '提供時間帯範囲外です。注文しますか。'
      this.messages['W_00020'] = '注文カートから削除します。'
      this.messages['I_00020'] = '選択した商品をカートに格納しました。'
      this.messages['I_00010'] = '{0}座席を選択します。'
      this.messages['E_00040'] = '注文カートにデータがありません。'
      this.messages['E_00050'] = '注文明細書き込みに失敗しました。'
      this.messages['E_00130'] = '未来日の入力はできません。'
      this.messages['E_00160'] = '前回確定よりも前の日付は入力できません。'
      this.messages['E_00070'] = '該当のデータはありません。'
      this.messages['E_00170'] = '使用中の座席はグループ設定できません。'
      this.messages['E_00200'] = '受付処理に失敗しました。'
      this.messages['E_00140'] = '棚卸数が入力されているデータがありません。'
      this.messages['E_00080'] = 'DBの処理に失敗しました。'
      this.messages['I_00070'] = '{0}が完了しました。'
      this.messages['E_00220'] = '注文可能数登録に失敗しました。'
      this.messages['I_00100'] = '{0}を行いました。'
      this.messages['I_00200'] = '入力した内容で注文可能数を登録しました。'
      this.messages['E_00190'] = '商品セットが選択されていません。'
      this.messages['E_00150'] = 'No{0}は、差異数に対する内訳が正しく入力されていません。'

      this.messages['W_00060'] = '差異数と内訳数が一致していません。よろしいですか？'
      this.messages['W_00070'] = '人数が全て入力されていません。よろしいですか？'
      this.messages['E_00060'] = '注文履歴がありません。'
      this.messages['E_00180'] = '注文可能数を超えたため注文できません。'
      this.messages['I_00110'] = '入力した内容で注文可能数を登録しました。'
      this.messages['I_00060'] = '棚卸確定を行い、棚卸数を在庫数に反映します。よろしいですか？'
      this.messages['E_00210'] = '商品分類が選択されていません。'
      this.messages['E_00230'] = '注文はひとつの座席しか選択できません。'
      this.messages['E_00240'] = '受付処理をしてから注文してください。'
      this.messages['E_00250'] = '注文が発生しているため受付変更できません。'
      this.messages['W_00080'] = '伝票印刷が異常終了しました。出力状況を確認してください。'

      this.init = true
    }
    var message = this.messages[id]
    if (!message && message !== '') {
      return id
    }
    if (args) {
      if (typeof args === 'object' && args.length) {
        for (var i = 0; i < args.length; i++) {
          var pattern = new RegExp('\\{' + i + '\\}', 'g')
          message = message.replace(pattern, args[i])
        }
      } else {
        message = message.replace(/\{0\}/g, args)
      }
    }
    return message
  },
  message: function(id, args) {
    if (id.indexOf('E') === 0) {
      return Notify({
        message: this.get(id, args),
        type: 'danger'
      })
    } else if (id.indexOf('W') === 0) {
      return Notify({
        message: this.get(id, args),
        type: 'warning'
      })
    } else if (id.indexOf('I') === 0) {
      return Notify({
        message: this.get(id, args),
        type: 'success'
      })
    } else {
      return Notify({
        message: this.get(id, args),
        type: 'primary'
      })
    }
  },
  messageBox: function(id, args) {
    if (id.indexOf('E') === 0) {
      return Dialog.alert({
        title: '',
        message: this.get(id, args),
        confirmButtonText: 'OK',
        theme: 'van-button--small'
      })
    } else if (id.indexOf('W') === 0) {
      return Dialog.confirm({
        title: '',
        message: this.get(id, args),
        confirmButtonText: 'OK',
        cancelButtonText: 'キャンセル'
      })
    } else if (id.indexOf('I') === 0) {
      return Dialog.confirm({
        title: '',
        message: this.get(id, args),
        confirmButtonText: 'OK',
        cancelButtonText: 'キャンセル'
      })
    } else {
      return Dialog.alert({
        title: this.get(id, args),
        message: 'message.messageInfoTitle',
        confirmButtonText: 'message.messageOkButton',
        theme: 'el-button--mini'
      })
    }
  },
  messageNew: function(id, args, _this, site) {
    const listE = ['氏名', '電話番号', '受取予定日時', '商品メニュー注文番号']
    const listI = ['I_00010', 'I_00020', 'I_00030', 'I_00200', 'I_00050', 'I_00060']
    const listW = ['W_00010', 'W_00020']
    const listWMsg = ['W_00080']
    if (id.indexOf('E') === 0 && id !== 'E_00020') {
      _this.message = this.get(id, args)
    } else if (id === 'E_00020' && listE.indexOf(args) !== -1) {
      _this.message = this.get(id, args)
    } else if (id === 'E_00020' && listE.indexOf(args) === -1) {
      _this[site] = this.get(id, args)
    } else if (listI.indexOf(id) !== -1) {
      return Dialog.confirm({
        title: '',
        message: this.get(id, args),
        confirmButtonText: 'OK',
        showCancelButton: false
      })
    } else if (listW.indexOf(id) !== -1) {
      return Dialog.confirm({
        title: '',
        message: this.get(id, args),
        confirmButtonText: 'OK',
        cancelButtonText: 'キャンセル'
      })
    } else if (id === 'W_00060' || id === 'W_00070') {
      return Dialog.confirm({
        title: '',
        message: this.get(id, args),
        confirmButtonText: 'OK',
        cancelButtonText: 'キャンセル'
      })
    } else if (id === 'I_00070' || id === 'I_00100') {
      return Notify({
        message: this.get(id, args),
        type: 'success'
      })
    } else if (listWMsg.indexOf(id) !== -1) {
      _this.message = this.get(id, args)
    }
  }
}

export default new BusinessMessage()
