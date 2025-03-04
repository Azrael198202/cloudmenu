using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       受付テーブル店内作成
    *****/
    public class ReqIReception : ReqMdBase
    {
        public List<SelectedSeatList> selectedSeatList { get; set; }//選択中座席リスト
        public string selectedSeatOnoff { get; set; }//選択中座席有無
        public string seatSelectMode { get; set; }//座席選択モード
        public string menuLineNumber { get; set; }//商品メニュー分類表示順
        public string menuOrderNumber { get; set; }//商品メニュー注文番号
        public string receptionKbn { get; set; }//受付区分
        public List<SeatList> seatList { get; set; }//座席一覧
    }
    /*****
    *       選択中座席リスト
    *****/
    public class SelectedSeatList
    {
        public string selectedSeatNumber { get; set; }//選択中座席番号
    }
    /*****
    *       座席一覧
    *****/
    public class SeatList
    {
        public string seatLevel { get; set; }//階層番号
        public string seatNumber { get; set; }//座席番号
        public string receptionNumber { get; set; }//受付番号
        public byte? receptionBranchNumber { get; set; }//受付枝番
        public string seatStatusKbn { get; set; }//座席状況区分
        public byte? seatPeopleMan { get; set; }//男性人数
        public byte? seatPeopleWoman { get; set; }//女性人数
        public byte? seatPeopleChildren { get; set; }//子供人数
        public string receptionRemarks { get; set; }//備考
        public string selectedSeatFlg { get; set; }//選択座席フラグ
        public string groupMode { get; set; }//グループ設定モード
    }
}
