using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TEquipmentConsumption
    {
        public string StoreNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string EquipmentConsumptionDate { get; set; }
        public byte EquipmentConsumptionTimes { get; set; }
        public string EquipmentConsumptionKbn { get; set; }
        public short? EquipmentConsumptionQuantity { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
