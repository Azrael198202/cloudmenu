using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MProductSet
    {
        public string StoreNumber { get; set; }
        public string ProductNumber { get; set; }
        public string ProductSetNumber { get; set; }
        public string ProductSetName { get; set; }
        public uint ProductPrice { get; set; }
        public string ProductTaxKbn { get; set; }
        public byte? ProductLimitedQuantity { get; set; }
        public string DelFlg { get; set; }
    }
}
