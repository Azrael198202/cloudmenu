using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *      商品分類のリスト
    *****/
    public class DbExtShohinType
    {
        // 商品番号
        public string productNumber { get; set; }
        // 商品セット番号
        public string productSetNumber { get; set; }
        // 商品名称（中国語）
        public string productNameCh { get; set; }
        // 商品セット名称
        public string productSetName { get; set; }
        // 注文可能数
        public int productLimitedQuantity { get; set; }
        //セット品有無フラグ
        public string setExistenceFlg { get; set; }
        // 数量限定区分
        public string limitedKbn { get; set; }

    }
}
