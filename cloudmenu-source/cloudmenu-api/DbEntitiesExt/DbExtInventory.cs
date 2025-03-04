using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *
    *****/
    public class DbExtInventory
    {
        // 対象名称
        public string targetName { get; set; }
        // 対象番号
        public string targetNumber { get; set; }
        // 前棚卸日
        public string lastDate { get; set; }
        // 前確定数
        public int lastQuantity { get; set; }
        // 入（入庫数）
        public int storingQuantity { get; set; }
        // 消（消費数）
        public int consumptionQuantity { get; set; }
        // 論理在庫数論理在庫数
        public int stockQuantity { get; set; }
    }
}
