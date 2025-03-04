using System.Linq;
using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.LwUtils;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    * 座席最新データ検索
    *****/
    public class RpnMdSeatCurrentInfos : RpnMdBase
    {
        //座席総合情報
        public List<DbExtSeatSumInfos> seatSumInfoList { get; set; } = new List<DbExtSeatSumInfos>();
        //持ち帰り総合情報
        public List<DbExtSeatSumInfos> takeOutSumInfoList { get; set; } = new List<DbExtSeatSumInfos>();
        //選択されている受付情報
        public DbExtSeatSumInfos selectedSeatTakeoutRecpInfo { get; set; } = new DbExtSeatSumInfos();
        //選択されている受付の注文一覧
        public List<DbExtOrderDetail> selectingOrderList { get; set; } = new List<DbExtOrderDetail>();
        // 通知情報リスト
        public List<NotifyEntity> notifyList { get; set; } = new List<NotifyEntity>();

        //ホール利用情報
        public int hallCount => this.seatSumInfoList.Count(st => st.SeatKbn == CategoryConst.C_003_座席区分.ctg_001_ホール);
        public int hallEmptyCount => this.seatSumInfoList.Count(st => (st.SeatKbn == CategoryConst.C_003_座席区分.ctg_001_ホール && String.IsNullOrEmpty(st.ReceptionNumber)));
        public int hallUsingCount => this.seatSumInfoList.Count(st => (st.SeatKbn == CategoryConst.C_003_座席区分.ctg_001_ホール && !String.IsNullOrEmpty(st.ReceptionNumber)));
        public int hallNotifyCount { get; set; } = 0;
        //個室利用情報
        public int roomCount => this.seatSumInfoList.Count(st => st.SeatKbn == CategoryConst.C_003_座席区分.ctg_003_個室);
        public int roomEmptyCount => this.seatSumInfoList.Count(st => (st.SeatKbn == CategoryConst.C_003_座席区分.ctg_003_個室 && String.IsNullOrEmpty(st.ReceptionNumber)));
        public int roomUsingCount => this.seatSumInfoList.Count(st => (st.SeatKbn == CategoryConst.C_003_座席区分.ctg_003_個室 && !String.IsNullOrEmpty(st.ReceptionNumber)));
        public int roomNotifyCount { get; set; } = 0;
        //
        public int refeshIntervalTime { get; set; } = 3500;
    }
}
