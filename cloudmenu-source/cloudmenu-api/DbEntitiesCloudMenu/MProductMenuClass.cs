using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MProductMenuClass
    {
        public string StoreNumber { get; set; }
        public string ProductMenuNumber { get; set; }
        public byte ProductMenuLineNumber { get; set; }
        public string ProductMenuClassKbn { get; set; }
        public string ProductMenuNovisibleFlg { get; set; }
        public string ProductMenuTakeoutFlg { get; set; }
    }
}
