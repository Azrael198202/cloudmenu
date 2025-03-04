using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MProductMenuItem
    {
        public string StoreNumber { get; set; }
        public string ProductMenuNumber { get; set; }
        public byte ProductMenuLineNumber { get; set; }
        public string ProductMenuOrderNumber { get; set; }
        public string ProductNumber { get; set; }
    }
}
