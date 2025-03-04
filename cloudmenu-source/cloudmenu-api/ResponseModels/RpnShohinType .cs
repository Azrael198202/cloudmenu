using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *     
    *****/
    public class RpnShohinType : RpnMdBase
    {
        // 商品分類区分の商品リスト
        public List<DbExtShohinType> productTypeKbnList { get; set; } = new List<DbExtShohinType>();
    }
}
