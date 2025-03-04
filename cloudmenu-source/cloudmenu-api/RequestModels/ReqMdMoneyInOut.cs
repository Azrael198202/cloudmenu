using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdMoneyInOut : ReqMdBase
    {
        public string MoneyInoutNumber { get; set; }
        public string MoneyInoutDatetime { get; set; }
        public uint? MoneyInoutPrice { get; set; }
        public string MoneyInoutKbn { get; set; }
        public string MoneyInoutReasonKbn { get; set; }
        public string MoneyInoutRemarks { get; set; }
        // public DbExtMoneyInOutInfo newMoneyInout = new DbExtMoneyInOutInfo();
    }
}
