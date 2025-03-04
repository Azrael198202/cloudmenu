using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *       システムメニューマスタ
    *****/
    public class RpnSystemMenuOrder : RpnMdBase
    {
        //メニューのリスト
        public List<DbExtSystemMenuReception> menuList { get; set; } = new List<DbExtSystemMenuReception>();
    }
}
