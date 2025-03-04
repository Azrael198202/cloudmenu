using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.LwUtils;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *  座席総合情報
    *****/
    public class DbExtSeatSumInfos
    {
        //--- 座席情報 -----------------------------------------------------
        public string StoreNumber { get; set; }
        public string SeatLevel { get; set; }//階層番号
        public string SeatNumber { get; set; }//座席番号
        public string SeatKbn { get; set; }//座席区分
        public string SeatName { get; set; }//座席名
        public byte? SeatQuantity { get; set; } //座席数
        public ushort? SeatSizeVertical { get; set; }//サイズ縦
        public ushort? SeatSizeHorizontal { get; set; }//サイズ横
        public ushort? SeatPositionVertical { get; set; }//座席位置縦
        public ushort? SeatPositionHorizontal { get; set; }//座席位置横

        //--- 受付情報 -----------------------------------------------------
        public string ReceptionNumber { get; set; }//受付番号
        public ushort ReceptionBranchNumber { get; set; }//受付枝番
        public string ReceptionKbn { get; set; }//受付区分
        public DateTime? SeatStartDate { get; set; }//着席日時
        public DateTime? SeatEndDate { get; set; }//退席日時
        public string SeatStatusKbn { get; set; }//座席状況区分
        public byte SeatPeopleMan { get; set; }//男性人数
        public byte SeatPeopleWoman { get; set; }//女性人数
        public byte SeatPeopleChildren { get; set; }//子供人数
        public string ReceptionRemarks { get; set; }//備考
        public string CustomerNumber { get; set; }//顧客コード
        public string TakeoutName { get; set; }//持帰り氏名
        public string TakeoutTel { get; set; }//持帰り電話番号
        public DateTime? TakeoutReceiptTime { get; set; }//持帰り受取予定日時

        //--- 顧客情報 -----------------------------------------------------
        public string customRepresentName { get; set; }
        //受付代表者電話番号
        public string customRepresentTel { get; set; }


        //--- Computed -----------------------------------------------------
        //受付区分名
        public string receptionKbnName => ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト ? "持帰" : "店内";
        //受付代表者名
        // 全人数
        public int SeatPelpleAll => SeatPeopleMan + SeatPeopleWoman + SeatPeopleChildren;
        //着席からの経過時間
        public string timePassedSinceStart => ReceptionKbn == CategoryConst.C_009_受付区分.ctg_002_テイクアウト ? "" : (DateTime.Now.Subtract(SeatStartDate ?? DateTime.Now).ToString(@"hh\:mm"));
        public string seatStartDateHm => SeatStartDate?.ToString("HH:mm");
        public string takeoutReceiptTimeHm => TakeoutReceiptTime?.ToString("HH:mm");

        //--- Need To set -----------------------------------------------------
        public long OrderPriceSum { get; set; } // 注文総額
        public int orderDetailCount { get; set; } // 注文件数
        public int orderedAllCount { get; set; } // (キャンセル含む)全ての注文件数
        /*
        * 0:空き,1:注文中,2:配膳中,3:待ちなし,4:通知メッセージ有り
        */
        public int SeatStatusNo { get; set; } = 0;//座席状況
        public string SeatMergeWith { get; set; }//座席共用(代表座席名)

        public string seatMergeAll { get; set; }//座席共用(全座席名)

        //サービス料計算
        public int seatServicePercent { get; set; } = 0;
        //　税率
        public int seatServiceTaxPercent { get; set; } = 0;
        public int discountTaxPercent { get; set; } = 0;
    }
}
