using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *プリンター伝票データ
    *****/
    public class RpnMdPrint : RpnMdBase
    {
        //選択座席のリスト
        public List<RpnMdPrintData> printerDataList { get; set; } = new List<RpnMdPrintData>();
    }
}
