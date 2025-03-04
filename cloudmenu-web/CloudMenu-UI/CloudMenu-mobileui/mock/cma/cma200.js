module.exports = [
  {
    url: '/order/selectShohinTypeKbnData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        productTypeKbnList: [
          {
            productNumber: '0001', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '辣子鸡', // 商品名称（中国語）
            productSetName: '1人前', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '001', // 数量限定区分
            setExistenceFlg: '1' // セット品有無フラグ
          },
          {
            productNumber: '0002', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '酸辣土豆丝', // 商品名称（中国語）
            productSetName: '', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '001', // 数量限定区分
            setExistenceFlg: '0' // セット品有無フラグ
          },
          {
            productNumber: '0003', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '蒜蓉空心菜', // 商品名称（中国語）
            productSetName: '', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '001', // 数量限定区分
            setExistenceFlg: '0' // セット品有無フラグ
          },
          {
            productNumber: '0004', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '爆炒牛肚', // 商品名称（中国語）
            productSetName: '', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '001', // 数量限定区分
            setExistenceFlg: '0' // セット品有無フラグ
          },
          {
            productNumber: '0005', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '尖椒肥肠', // 商品名称（中国語）
            productSetName: '', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '900', // 数量限定区分
            setExistenceFlg: '0' // セット品有無フラグ
          },
          {
            productNumber: '0006', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '回锅肉', // 商品名称（中国語）
            productSetName: '', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '900', // 数量限定区分
            setExistenceFlg: '0' // セット品有無フラグ
          },
          {
            productNumber: '0007', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '地三鲜', // 商品名称（中国語）
            productSetName: '1人前', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '001', // 数量限定区分
            setExistenceFlg: '1' // セット品有無フラグ
          },
          {
            productNumber: '0007', // 商品番号
            productSetNumber: 1, // 商品セット番号
            productNameCh: '地三鲜', // 商品名称（中国語）
            productSetName: '2人前', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '001', // 数量限定区分
            setExistenceFlg: '1' // セット品有無フラグ
          },
          {
            productNumber: '0008', // 商品番号
            productSetNumber: 0, // 商品セット番号
            productNameCh: '青椒蛋饼', // 商品名称（中国語）
            productSetName: '', // 商品セット名称
            productLimitedQuantity: 1, // 注文可能数
            limitedKbn: '001', // 数量限定区分
            setExistenceFlg: '0' // セット品有無フラグ
          }
        ],
        msgList: [
          {
            msgCode: 'E_00010',
            msgItemId: '1',
            msgOption: '商品分類区分商品'
          }
        ]
      }
    }
  },
  {
    url: '/order/updateLimitedQuantity.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        msgList: [
          {
            msgCode: 'E_00220',
            msgItemId: '1',
            msgOption: ''
          }
        ]
      }
    }
  }
]
