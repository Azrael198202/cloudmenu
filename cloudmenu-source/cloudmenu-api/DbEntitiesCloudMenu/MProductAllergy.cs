using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MProductAllergy
    {
        public string StoreNumber { get; set; }
        public string ProductNumber { get; set; }
        public string ProductAllergiesKbn { get; set; }
        public string ProductAllergiesFlg { get; set; }
    }
}
