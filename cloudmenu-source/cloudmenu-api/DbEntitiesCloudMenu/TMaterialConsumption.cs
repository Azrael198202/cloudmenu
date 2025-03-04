using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TMaterialConsumption
    {
        public string StoreNumber { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialConsumptionDate { get; set; }
        public byte MaterialConsumptionTimes { get; set; }
        public string MaterialConsumptionKbn { get; set; }
        public short? MaterialConsumptionQuantity { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
