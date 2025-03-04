using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdSeatPrintCookChit : ReqMdBase
    {
        public string selectedRecpNumber { get; set; }
        // 調理指示伝票の注文データリスト
        public List<TOrder> ordersToPrint { get; set; }
        // public List<ReqMdSeatPrintOrder> ordersToPrint { get; set; }
    }
}
