module.exports = [
  {
    url: '/order/selectShohinDetails.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        shohinDetailsList: [
          {
            productNumber: '101',
            setNumber: '1',
            image: '/images/image.jpg',
            nameCh: '青椒肉丝',
            nameJp: 'ピーマンと豚肉細切炒め',
            calorie: 1200,
            offertimeFrom: '17:00',
            offertimeTo: '20:00',
            price: 2000,
            introduction:
              'ピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒めピーマンと豚肉細切炒め',
            pickupFlg: '1',
            existenceFlg: '1',
            courseFlg: '1',
            preparationFlg: '0',
            remarks: '备注',
            typeKbn: '',
            setNumberList: [
              { setName: '一人前', setPrice: 1200 },
              { setName: '两人前', setPrice: 1400 },
              { setName: '三人前', setPrice: 1600 }
            ]
          }
        ],

        allergiesList: [
          // 商品过敏分类
          { allergiesKbn: '001', allergiesFlg: '0' },
          { allergiesKbn: '002', allergiesFlg: '0' },
          { allergiesKbn: '003', allergiesFlg: '0' },
          { allergiesKbn: '004', allergiesFlg: '0' },
          { allergiesKbn: '005', allergiesFlg: '0' },
          { allergiesKbn: '006', allergiesFlg: '1' },
          { allergiesKbn: '007', allergiesFlg: '1' },
          { allergiesKbn: '008', allergiesFlg: '1' },
          { allergiesKbn: '009', allergiesFlg: '1' }
        ],

        storeList: [
          {
            // 过敏性标记
            allergiesDisplayFlg: '1'
          }
        ],
        courseList: [
          { courseProductName: '前菜', courseProductNameJp: 'デザート', courseOrder: '1' },
          { courseProductName: '主菜', courseProductNameJp: '季節サラダ', courseOrder: '2' },
          { courseProductName: '点心', courseProductNameJp: 'お任せ牛肉料理', courseOrder: '3' }
        ],
        msgList: [{ msgCode: 'E_00010', msgOption: '商品マスタ（詳細）', msgItemId: '' }]
      }
    }
  },
  {
    // url: '/order/searchKubun.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        categoryList: [
          { categoryKbn: '001', categoryKbnName: '鶏卵' },
          { categoryKbn: '002', categoryKbnName: '牛乳' },
          { categoryKbn: '003', categoryKbnName: '小麦' },
          { categoryKbn: '004', categoryKbnName: 'ピーナツ' },
          { categoryKbn: '005', categoryKbnName: '果物類' },
          { categoryKbn: '006', categoryKbnName: '魚卵' },
          { categoryKbn: '007', categoryKbnName: '甲殻類' },
          { categoryKbn: '008', categoryKbnName: 'ナッツ類' },
          { categoryKbn: '009', categoryKbnName: 'ソバ' },
          { categoryKbn: '010', categoryKbnName: '魚類' },
          { categoryKbn: '011', categoryKbnName: 'その他' }
        ]
      }
      // return{
      //     status:601,
      //     msgList:[
      //         {msgCode:'E_00010',msgItemId:'',msgOption:'区分取得'}
      //     ]
      // }
    }
  }
]
