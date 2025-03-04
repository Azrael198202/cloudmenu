using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *     v_product_menu_item
    *****/
    public class RpnInsertOrder : RpnMdBase
    {
        //注文管理リスト
        public List<DbExtOrderReception> orderList { get; set; } = new List<DbExtOrderReception>();
    }
}
