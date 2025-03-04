using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TProductConsumption
    {
        public string StoreNumber { get; set; }
        public string ProductNumber { get; set; }
        public string ProductConsumptionDate { get; set; }
        public byte ProductConsumptionTimes { get; set; }
        public string ProductConsumptionKbn { get; set; }
        public short? ProductConsumptionQuantity { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
