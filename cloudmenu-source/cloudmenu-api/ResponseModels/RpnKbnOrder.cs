using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *     区分マスタ
    *****/
    public class RpnKbnOrder : RpnMdBase
    {
        //区分分類のリスト
        public List<DbExtKbnReception> categoryList { get; set; } = new List<DbExtKbnReception>();
    }
}
