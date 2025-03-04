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
    public class RpnMdOrder : RpnMdBase
    {
        //選択座席のリスト
        public List<DbExtSeatReception> seatList { get; set; } = new List<DbExtSeatReception>();
    }
}
