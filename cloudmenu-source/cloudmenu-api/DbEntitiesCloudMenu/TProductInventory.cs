using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TProductInventory
    {
        public string StoreNumber { get; set; }
        public string ProductInventoryDate { get; set; }
        public string ProductNumber { get; set; }
        public short? ProductInventoryQuantity { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
