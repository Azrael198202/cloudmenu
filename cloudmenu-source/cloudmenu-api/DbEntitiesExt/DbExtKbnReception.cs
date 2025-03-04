using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *      区分分類のリスト
    *****/
    public class DbExtKbnReception
    {
        public string CategoryKbn { get; set; }//区分値
        public string CategoryKbnName { get; set; }//区分値名称
        public string CategoryKbnAbbr { get; set; }//区分値略称
    }
}
