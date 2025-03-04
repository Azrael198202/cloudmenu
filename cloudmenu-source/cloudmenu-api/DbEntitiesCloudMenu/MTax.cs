using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MTax
    {
        public string StoreNumber { get; set; }
        public string TaxKbn { get; set; }
        public string TaxDateStart { get; set; }
        public string TaxDateEnd { get; set; }
        public byte TaxPercent { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
