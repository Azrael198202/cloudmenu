using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       受付テーブル持ち帰り作成
    *****/
    public class ReqIReceptionTakeOut : ReqMdBase
    {
        // 氏名
        public string takeoutName { get; set; }
        // 電話番号
        public string takeoutTel { get; set; }
        // 受取予定日時
        public string takeoutReceiptTime { get; set; }
        public List<SelectedSeatList> selectedSeatList { get; set; }//選択中座席リスト
        public string selectedSeatOnoff { get; set; }//選択中座席有無
        public string seatSelectMode { get; set; }//座席選択モード
        public string menuLineNumber { get; set; }//商品メニュー分類表示順
        public string menuOrderNumber { get; set; }//商品メニュー注文番号
        public string receptionKbn { get; set; }//受付区分
        public List<SeatList> seatList { get; set; }//座席一覧

    }
}
