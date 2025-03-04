using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       受付テーブルのリスト
    *****/
    public class DbExtReception
    {
        public string SeatLevel { get; set; }//階層番号
        public string SeatNumber { get; set; }//座席番号
        public string receptionNumber { get; set; }//受付番号
        public int? receptionBranchNumber { get; set; }//受付枝番
        public string receptionKbn { get; set; }//受付区分
    }
}
