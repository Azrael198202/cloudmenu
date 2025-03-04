using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *
    *****/
    public class DbExtStoring
    {
        // 分類区分
        public string categoryKbn { get; set; }
        // 分類名
        public string categoryName { get; set; }
        // 入庫数
        public int storingQuantity { get; set; }
        // 対象名称
        public string targetName { get; set; }
        // 単位区分名称
        public string unitName { get; set; }
        // 対象番号
        public string targetNumber { get; set; }
        // 単位区分
        public string unitKbn { get; set; }

    }
}
