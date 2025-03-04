using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdSeatPrintOrder
    {
        public string ReceptionNumber { get; set; }
        public string OrderVoucherNumber { get; set; }
        public string OrderVoucherDetails { get; set; }
    }
}
