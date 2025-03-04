using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *       商品メニューマスタ
    *****/
    public class RpnMenuOrder : RpnMdBase
    {
        //選択座席のリスト
        public List<DbExtMenuReception> menuclassList { get; set; } = new List<DbExtMenuReception>();
    }
}
