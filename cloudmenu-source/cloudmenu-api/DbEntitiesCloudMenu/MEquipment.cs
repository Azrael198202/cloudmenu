using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MEquipment
    {
        public string StoreNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentTypeKbn { get; set; }
        public string EquipmentUnitKbn { get; set; }
        public string DelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
