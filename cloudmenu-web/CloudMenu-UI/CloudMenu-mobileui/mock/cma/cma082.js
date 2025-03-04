module.exports = [
  {
    url: '/order/insertReceptionTakeoutData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        receptionList: [
          { seatLevel: '', seatNumber: '', receptionNumber: '00001', receptionBranchNumber: '01', seatStatusKbn: '' },
          { seatLevel: '', seatNumber: '', receptionNumber: '', receptionBranchNumber: '', seatStatusKbn: '' },
          { seatLevel: '', seatNumber: '', receptionNumber: '', receptionBranchNumber: '', seatStatusKbn: '' }
        ],
        msgList: [{ msgCode: 'E_00200', msgOption: '受付処理に失敗しました。', msgItemId: '' }]
      }
    }
  }
]
