using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *      商品メニュー分類のリスト
    *****/
    public class DbExtMenuReception
    {
        public string MenuNumber { get; set; }//商品メニュー管理番号
        public byte MenuLineNumber { get; set; }//商品メニュー分類表示順
        public string MenuClassName { get; set; }//商品メニュー分類名称
        public string MenuTakeoutFlg { get; set; }//持ち帰り非表示フラグ
    }
}
