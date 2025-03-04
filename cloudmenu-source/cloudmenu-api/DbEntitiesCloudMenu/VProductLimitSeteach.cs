using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class VProductLimitSeteach
    {
        public string StoreNumber { get; set; }
        public string ProductNumber { get; set; }
        public string ProductSetNumber { get; set; }
        public decimal? SumProductLimitedQuantity { get; set; }
        public decimal SumOrderQuantity { get; set; }
        public decimal? LimitedQuantity { get; set; }
    }
}
