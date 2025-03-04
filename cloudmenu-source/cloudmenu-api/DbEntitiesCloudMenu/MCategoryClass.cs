using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MCategoryClass
    {
        public string StoreNumber { get; set; }
        public string CategoryClassNumber { get; set; }
        public string CategoryClassName { get; set; }
        public string CategorySystemFlg { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryClassOptionValues { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
