using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class VProductInventoryNow
    {
        public string StoreNumber { get; set; }
        public string ProductNumber { get; set; }
        public short? ProductStockQuantity { get; set; }
        public string 前回棚卸日 { get; set; }
        public long 前回確定数 { get; set; }
        public decimal 入庫 { get; set; }
        public decimal 消費 { get; set; }
        public decimal 論理在庫数 { get; set; }
    }
}
