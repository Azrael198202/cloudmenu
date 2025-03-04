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
    public class RpnChoiceOrder : RpnMdBase
    {
        //選択分類商品のリスト
        public List<DbExtBChoiceReception> shohinList { get; set; } = new List<DbExtBChoiceReception>();
    }
}
