using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TMoneyInout
    {
        public string StoreNumber { get; set; }
        public string MoneyInoutNumber { get; set; }
        public DateTime MoneyInoutDatetime { get; set; }
        public uint? MoneyInoutPrice { get; set; }
        public string MoneyInoutKbn { get; set; }
        public string MoneyInoutReasonKbn { get; set; }
        public string MoneyInoutRbFlg { get; set; }
        public string MoneyInoutOriginNumber { get; set; }
        public string MoneyInoutRemarks { get; set; }
        public string ClosingFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
