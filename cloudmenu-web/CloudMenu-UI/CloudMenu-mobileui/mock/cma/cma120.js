module.exports = [
  {
    url: '/order/selectOrderData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        dataCount: 0,
        msgList: [{ msgCode: 'E_00010', msgOption: '注文データ', msgItemId: '' }]
      }
    }
  },
  {
    url: '/order/selectOrderHistoryData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        orderHistoryList: [
          {
            orderDatetime: '18:35',
            orderVoucherNumber: '0001',
            orderVoucherDetails: '00010001',
            orderProductNumber: '0001',
            orderProductSetNumber: '0001',
            orderProductNameCh: '青椒肉丝',
            orderProductNameJp: '青椒肉丝',
            orderProductSetName: '1人前',
            orderProductTypeKbn: '0001',
            orderQuantity: 1,
            orderPrice: 1600,
            orderDiscountPrice: 1600,
            orderTakeoutFlg: null,
            orderRemarks: ''
          },
          {
            orderDatetime: '18:35',
            orderVoucherNumber: '0001',
            orderVoucherDetails: '00010001',
            orderProductNumber: '0001',
            orderProductSetNumber: '0001',
            orderProductNameCh: '青椒肉丝',
            orderProductNameJp: '青椒肉丝',
            orderProductSetName: '1人前',
            orderProductTypeKbn: '0001',
            orderQuantity: 1,
            orderPrice: 1600,
            orderDiscountPrice: 1600,
            orderTakeoutFlg: null,
            orderRemarks: '备注'
          },
          {
            orderDatetime: '18:40',
            orderVoucherNumber: '0001',
            orderVoucherDetails: '00010001',
            orderProductNumber: '0001',
            orderProductSetNumber: '0001',
            orderProductNameCh: '青椒肉丝2',
            orderProductNameJp: '青椒肉丝2',
            orderProductSetName: '1人前',
            orderProductTypeKbn: '0001',
            orderQuantity: 1,
            orderPrice: 1600,
            orderDiscountPrice: 1600,
            orderTakeoutFlg: '1',
            orderRemarks: '备注'
          },
          {
            orderDatetime: '20:40',
            orderVoucherNumber: '0001',
            orderVoucherDetails: '00010001',
            orderProductNumber: '0001',
            orderProductSetNumber: '0001',
            orderProductNameCh: '泡椒牛蛙',
            orderProductNameJp: 'ウシガエルとビーフン山椒スープ煮物',
            orderProductSetName: '',
            orderProductTypeKbn: '0001',
            orderQuantity: 1,
            orderPrice: 1600,
            orderDiscountPrice: 1600,
            orderTakeoutFlg: '1',
            orderRemarks: '备注'
          }
        ],
        orderQuantityTotal: 15,
        orderPriceTotal: 8200,
        msgList: [{ msgCode: 'E_00050', msgOption: '注文明細書き込みに失敗しました。', msgItemId: '' }]
      }
    }
  },
  {
    url: '/order/cookingInstFlgUpdate.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        dataCount: 0,
        msgList: [{ msgCode: 'E_00010', msgOption: '注文データ', msgItemId: '' }]
      }
    }
  },
  {
    url: '/order/cookingInstSlipPrt.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        dataCount: 0,
        msgList: [{ msgCode: 'E_00010', msgOption: '注文データ', msgItemId: '' }]
      }
    }
  }
]
