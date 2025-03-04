using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TEquipmentInventory
    {
        public string StoreNumber { get; set; }
        public string EquipmentInventoryDate { get; set; }
        public string EquipmentNumber { get; set; }
        public short? EquipmentInventoryQuantity { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
