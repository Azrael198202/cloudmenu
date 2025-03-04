using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       選択座席のリスト
    *****/
    public class DbExtSeatReception
    {
        public string seatLevel { get; set; }//階層番号
        public string seatNumber { get; set; }//座席番号
        public string seatKbn { get; set; }//座席区分
        public string seatName { get; set; }//座席名
        public string receptionNumber { get; set; }//受付番号
        public ushort? receptionBranchNumber { get; set; }//受付枝番
        public DateTime? seatStartDate { get; set; } // 受付時間
        public string seatStatusKbn { get; set; }//座席状況区分
        public byte? seatPeopleMan { get; set; }//男性人数
        public byte? seatPeopleWoman { get; set; }//女性人数
        public byte? seatPeopleChildren { get; set; }//子供人数
        public string receptionRemarks { get; set; }//備考
        public string receptionKbn { get; set; }//注文区分
        public string takeoutName { get; set; }//持帰り氏名
        public string takeoutTel { get; set; }//持帰り電話番号
        public DateTime? takeoutReceiptTime { get; set; }//持帰り受取予定日時
    }
}
