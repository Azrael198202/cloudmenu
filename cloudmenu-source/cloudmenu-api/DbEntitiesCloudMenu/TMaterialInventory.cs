using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TMaterialInventory
    {
        public string StoreNumber { get; set; }
        public string MaterialInventoryDate { get; set; }
        public string MaterialNumber { get; set; }
        public short? MaterialInventoryQuantity { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
