using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdTana : ReqMdBase
    {
        // 棚卸日
        public string inventoryDate { get; set; }
        // 品目区分
        public string itemKbn { get; set; }
        // 分類区分
        public string categoryKbn { get; set; }
    }
}
