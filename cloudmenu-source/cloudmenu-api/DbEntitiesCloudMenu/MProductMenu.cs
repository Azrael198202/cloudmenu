using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MProductMenu
    {
        public string StoreNumber { get; set; }
        public string ProductMenuNumber { get; set; }
        public string ProductMenuName { get; set; }
        public string ProductMenuApplyFlg { get; set; }
        public string DelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
