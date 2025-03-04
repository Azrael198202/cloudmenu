module.exports = [
  {
    url: '/order/selectShohinBunrui.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        menuclassList: [
          { menuClassName: '単品', menuLineNumber: 1, menuNumber: '030010' },
          { menuClassName: 'ランチ', menuLineNumber: 2, menuNumber: '030020' },
          { menuClassName: '定食', menuLineNumber: 3, menuNumber: '030030' },
          { menuClassName: '飲み物', menuLineNumber: 4, menuNumber: '030040' },
          { menuClassName: '春限定', menuLineNumber: 5, menuNumber: '030050' },
          { menuClassName: '春限定2', menuLineNumber: 6, menuNumber: '030050' },
          { menuClassName: '春限定3', menuLineNumber: 7, menuNumber: '030050' },
          { menuClassName: '春限定4', menuLineNumber: 8, menuNumber: '030050' },
          { menuClassName: '春限定5', menuLineNumber: 9, menuNumber: '030050' },
          { menuClassName: '春限定6', menuLineNumber: 10, menuNumber: '030050' },
          { menuClassName: '春限定7', menuLineNumber: 11, menuNumber: '030050' }
        ],
        msgList: [{ msgCode: 'E_00010', msgOption: '商品メニュー分類', msgItemId: '' }]
      }
    }
  }
]
