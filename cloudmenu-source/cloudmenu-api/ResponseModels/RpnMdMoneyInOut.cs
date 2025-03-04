using System;
using cloudmenu_api.LwUtils;
using cloudmenu_api.DbentitiesExt;
using System.Collections.Generic;

namespace cloudmenu_api.ResponseModels
{

    /*****
    *プリンター伝票データ
    *****/
    public class RpnMdMoneyInOut : RpnMdBase
    {
        public List<DbExtMoneyInOutInfo> moneyInOutList { get; set; } = new List<DbExtMoneyInOutInfo>();
    }
}
