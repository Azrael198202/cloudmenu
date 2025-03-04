using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MMaterial
    {
        public string StoreNumber { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialName { get; set; }
        public string MaterialTypeKbn { get; set; }
        public string MaterialUnitKbn { get; set; }
        public string DelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
