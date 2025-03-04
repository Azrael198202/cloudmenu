module.exports = [
  {
    url: '/order/serachCTaxKubun.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        ctaxList: [
          {
            taxKbn: '010', // 店内税率
            taxPercent: '0.9'
          },
          {
            taxKbn: '020',
            taxPercent: '0.5'// 持ち帰り税率
          }
        ],
        msgList: [{ msgCode: 'E_00010', msgOption: '税区分取得' }]
      }
    }
  }
]
