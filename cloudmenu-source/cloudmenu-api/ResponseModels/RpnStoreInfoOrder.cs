using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *       店舗情報マスタ
    *****/
    public class RpnStoreInfoOrder : RpnMdBase
    {
        //メニューのリスト
        public List<DbExtStoreInfoReception> storeList { get; set; } = new List<DbExtStoreInfoReception>();
    }
}
