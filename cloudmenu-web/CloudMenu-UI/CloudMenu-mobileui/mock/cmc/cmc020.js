module.exports = [
  {
    url: '/order/searchStoreInfo.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        storeList: [
          {
            name: '店铺名',
            address: '住所',
            tel: '电话番号',
            url: 'URL',
            weekdaysFrom: '営業時間平日From',
            weekdaysTo: '営業時間平日To',
            saturdayFrom: '営業時間土曜日From',
            saturdayTo: '営業時間土曜日To',
            holidaysFrom: '営業時間土曜日To',
            holidaysTo: '営業時間日祝From',
            seatQuantity: 5,
            allergiesDisplayFlg: 'アレルギー表示有無フラグ',
            logoImage: '/images/logo1.png',
            image: '/images/page-index.jpg',
            introduction:
              '主理闽、川等菜系以及金牌健康素食和传统客家菜，专注于原汁原味以及均衡营养，装修精致典雅，拥有宴会厅、客饭厅、零点厅、教工餐厅及17个包厢，可容纳700人同时用餐，配有中央和分体式空调、有线电视、无线网络等完善设施。',
            staffWord: '一言'
          },
          {
            name: '店铺名',
            address: '住所',
            tel: '电话番号',
            url: 'URL',
            weekdaysFrom: '営業時間平日From',
            weekdaysTo: '営業時間平日To',
            saturdayFrom: '営業時間土曜日From',
            saturdayTo: '営業時間土曜日To',
            holidaysFrom: '営業時間土曜日To',
            holidaysTo: '営業時間日祝From',
            seatQuantity: 5,
            allergiesDisplayFlg: 'アレルギー表示有無フラグ',
            logoImage: '../../src/assets/images/logo1.png',
            image: '../../src/assets/images/page-index.jpg',
            introduction: '店舗紹介',
            staffWord: '一言'
          },
          {
            name: '店铺名',
            address: '住所',
            tel: '电话番号',
            url: 'URL',
            weekdaysFrom: '営業時間平日From',
            weekdaysTo: '営業時間平日To',
            saturdayFrom: '営業時間土曜日From',
            saturdayTo: '営業時間土曜日To',
            holidaysFrom: '営業時間土曜日To',
            holidaysTo: '営業時間日祝From',
            seatQuantity: 5,
            allergiesDisplayFlg: 'アレルギー表示有無フラグ',
            logoImage: '../../src/assets/images/logo1.png',
            image: '../../src/assets/images/page-index.jpg',
            introduction: '店舗紹介',
            staffWord: '一言'
          },
          {
            name: '店铺名',
            address: '住所',
            tel: '电话番号',
            url: 'URL',
            weekdaysFrom: '営業時間平日From',
            weekdaysTo: '営業時間平日To',
            saturdayFrom: '営業時間土曜日From',
            saturdayTo: '営業時間土曜日To',
            holidaysFrom: '営業時間土曜日To',
            holidaysTo: '営業時間日祝From',
            seatQuantity: 5,
            allergiesDisplayFlg: 'アレルギー表示有無フラグ',
            logoImage: '../../src/assets/images/logo1.png',
            image: '../../src/assets/images/page-index.jpg',
            introduction: '店舗紹介',
            staffWord: '一言'
          },
          {
            name: '店铺名',
            address: '住所',
            tel: '电话番号',
            url: 'URL',
            weekdaysFrom: '営業時間平日From',
            weekdaysTo: '営業時間平日To',
            saturdayFrom: '営業時間土曜日From',
            saturdayTo: '営業時間土曜日To',
            holidaysFrom: '営業時間土曜日To',
            holidaysTo: '営業時間日祝From',
            seatQuantity: 5,
            allergiesDisplayFlg: 'アレルギー表示有無フラグ',
            logoImage: '../../src/assets/images/logo1.png',
            image: '../../src/assets/images/page-index.jpg',
            introduction: '店舗紹介',
            staffWord: '一言'
          }
        ]
      }
      // return{
      //     status: 601,
      //     msgList:[
      //        { msgCode: 'E_00010', msgOption: '座席データ全件',msgItemId:'' }
      //     ]}
    }
  }
]
