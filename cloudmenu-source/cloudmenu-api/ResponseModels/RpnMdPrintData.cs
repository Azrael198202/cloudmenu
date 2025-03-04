using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *注文の調理指示伝票
    *****/
    public class RpnMdPrintData : RpnMdBase
    {
        //選択座席のリスト
        public string printerUri { get; set; } = "";
        public string printerName { get; set; } = "";
        public List<string> printXmlDatas { get; set; } = new List<string>();
    }
}
