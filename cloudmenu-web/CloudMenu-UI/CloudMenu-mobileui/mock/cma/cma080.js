module.exports = [
  {
    url: '/order/selectZasekiAllData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        seatList: [
          {
            seatLevel: '1',
            seatNumber: '1',
            seatPeopleMan: 1,
            receptionNumber: '001',
            receptionBranchNumber: '001',
            seatPeopleWoman: 1,
            seatName: 'テーブル1',
            seatPeopleChildren: 1,
            seatStatusKbn: '002',
            receptionRemarks: '煎菜'
          },
          {
            seatLevel: '1',
            seatNumber: '2',
            seatPeopleMan: 1,
            receptionNumber: '001',
            receptionBranchNumber: '002',
            seatPeopleWoman: '1',
            seatName: 'テーブル2',
            seatPeopleChildren: 1,
            seatStatusKbn: '002',
            receptionRemarks: ''
          },

          {
            seatLevel: '1',
            seatNumber: '3',
            seatPeopleMan: 1,
            receptionNumber: '001',
            receptionBranchNumber: '002',
            seatPeopleWoman: 1,
            seatName: 'テーブル3',
            seatPeopleChildren: 1,
            seatStatusKbn: '002',
            receptionRemarks: ''
          },

          {
            seatLevel: '2',
            seatNumber: '4',
            seatPeopleMan: 3,
            receptionNumber: '001',
            receptionBranchNumber: '002',
            seatPeopleWoman: 4,
            seatName: 'テーブル4',
            seatPeopleChildren: 3,
            seatStatusKbn: '001',
            receptionRemarks: ''
          },

          {
            seatLevel: '2',
            seatNumber: '5',
            seatPeopleMan: 1,
            receptionNumber: '001',
            receptionBranchNumber: '002',
            seatPeopleWoman: 1,
            seatName: 'テーブル5',
            seatPeopleChildren: 1,
            seatStatusKbn: '001',
            receptionRemarks: ''
          },

          {
            seatLevel: '2',
            seatNumber: '6',
            seatPeopleMan: 1,
            receptionNumber: '003',
            receptionBranchNumber: '003',
            seatPeopleWoman: 1,
            seatName: 'テーブル6',
            seatPeopleChildren: 1,
            seatStatusKbn: '001',
            receptionRemarks: ''
          },

          {
            seatLevel: '3',
            seatNumber: '7',
            seatPeopleMan: 1,
            receptionNumber: '002',
            receptionBranchNumber: '002',
            seatPeopleWoman: 1,
            seatName: 'テーブル7',
            seatPeopleChildren: 1,
            seatStatusKbn: '001',
            receptionRemarks: ''
          },

          {
            seatLevel: '3',
            seatNumber: '8',
            seatPeopleMan: 1,
            receptionNumber: '001',
            receptionBranchNumber: '002',
            seatPeopleWoman: 1,
            seatName: 'テーブル8',
            seatPeopleChildren: 1,
            seatStatusKbn: '001',
            receptionRemarks: ''
          },

          {
            seatLevel: '3',
            seatNumber: '9',
            seatPeopleMan: 1,
            receptionNumber: '001',
            receptionBranchNumber: '002',
            seatPeopleWoman: 1,
            seatName: 'テーブル9',
            seatPeopleChildren: 1,
            seatStatusKbn: '001',
            receptionRemarks: ''
          },

          {
            seatLevel: '3',
            seatNumber: '10',
            seatPeopleMan: 1,
            receptionNumber: '001',
            receptionBranchNumber: '002',
            seatPeopleWoman: 1,
            seatName: 'テーブル10',
            seatPeopleChildren: 1,
            seatStatusKbn: '001',
            receptionRemarks: ''
          }
        ],
        msgList: [{ msgCode: 'E_00010', msgOption: '座席データ全件', msgItemId: '' }]
      }
    }
  },
  {
    url: '/order/insertReceptionInsideData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        receptionList: [
          { seatLevel: '', seatNumber: '', receptionNumber: '', receptionBranchNumber: '', seatStatusKbn: '' },
          { seatLevel: '', seatNumber: '', receptionNumber: '', receptionBranchNumber: '', seatStatusKbn: '' },
          { seatLevel: '', seatNumber: '', receptionNumber: '', receptionBranchNumber: '', seatStatusKbn: '' }
        ],
        msgList: [{ msgCode: 'E_00200', msgOption: '受付処理に失敗しました。', msgItemId: '' }]
      }
    }
  },
  {
    url: '/order/selectZasekiChoiceData.do',
    type: 'post',
    response: _ => {
      return {
        status: 200,
        selectSeatList: [
          {
            seatLevel: '3',
            seatNumber: '10',
            seatKbn: '003005',
            seatName: 'テーブル',
            receptionNumber: '001',
            receptionBranchNumber: '001',
            seatStatusKbn: '001',
            seatPeopleMan: 0,
            seatPeopleWoman: 0,
            seatPeopleChildren: 0,
            receptionRemarks: ''
          }
        ],
        msgList: [{ msgCode: 'E_00010', msgOption: '選択座席データ', msgItemId: '' }]
      }
    }
  }
]
