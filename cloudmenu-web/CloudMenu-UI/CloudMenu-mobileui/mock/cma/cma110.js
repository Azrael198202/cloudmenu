module.exports = [
  {
    url: '/order/insertOrderData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        msgList: [
          { msgCode: 'E_00050', msgItemId: '', msgOption: '' }
        ],
        orderList: [
          {
            orderDatetime: '20210623',
            orderVoucherNumber: '0001',
            orderVoucherDetails: '00010001',
            orderProductNumber: '0001',
            orderProductSetNumber: '0001',
            orderProductNameCh: '青椒肉丝',
            orderProductSetName: '0001',
            orderProductTypeKbn: '0001',
            orderQuantity: 1,
            orderTakeoutFlg: '1',
            orderRemarks: '备注',
            orderSlipFlg: '1'
          }
        ]
      }
    }
  },
  {
    url: '/order/cookingInstSlipPrt.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        msgList: [
          { msgCode: 'W_00080', msgItemId: '', msgOption: '' }
        ]
      }
    }
  }
]
