module.exports = [
  {
    url: '/order/searchKubun',
    type: 'post',
    response: config => {
      return {
        status: 200,
        categoryList: [
          {
            categoryKbn: '031001',
            categoryKbnName: '燕・翅・鮑・参・肚',
            categoryKbnAbbr: ''
          },
          {
            categoryKbn: '031002',
            categoryKbnName: '前菜',
            categoryKbnAbbr: ''
          },
          {
            categoryKbn: '031003',
            categoryKbnName: '海鮮・老上海',
            categoryKbnAbbr: ''
          },
          {
            categoryKbn: '031004',
            categoryKbnName: '家常小炒',
            categoryKbnAbbr: ''
          },
          {
            categoryKbn: '031005',
            categoryKbnName: '野菜・豆腐',
            categoryKbnAbbr: ''
          },
          {
            categoryKbn: '031006',
            categoryKbnName: 'スープ・羹',
            categoryKbnAbbr: ''
          },
          {
            categoryKbn: '031007',
            categoryKbnName: '麺・飯',
            categoryKbnAbbr: ''
          },
          {
            categoryKbn: '031008',
            categoryKbnName: '甜品・デザート',
            categoryKbnAbbr: ''
          }],
        msgList: [{ msgCode: 'E_00010', msgOption: '検索でエラーが発生しました。', msgItemId: '' }]
      }
    }
  }
]

