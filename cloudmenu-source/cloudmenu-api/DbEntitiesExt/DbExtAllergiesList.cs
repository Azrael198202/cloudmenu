using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *          商品アレルギー情報のリスト
    *****/
    public class DbExtAllergiesList
    {
        public string allergiesKbn { get; set; }// 商品アレルギー区分
        public string allergiesFlg { get; set; }// 商品アレルギーフラグ
    }
}
