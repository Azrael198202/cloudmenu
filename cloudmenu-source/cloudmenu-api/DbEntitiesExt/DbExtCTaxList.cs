using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *          税区分のリスト
    *****/
    public class DbExtCTaxList
    {
        public string taxKbn { get; set; }// 税区分
        public string taxPercent { get; set; }// 税率
    }
}
