using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    * 座席状況一覧の通知情報
    *****/
    public class NotifyEntity
    {
        /*
        * 1:ホール 2:個室 3:持ち帰り
        */
        public int notifyKbn { get; set; }//通知座席・持ち帰り名
        public string seatTakeoutName { get; set; }//通知座席・持ち帰り名
        public string notifyMsg { get; set; }//通知内容
    }
}
