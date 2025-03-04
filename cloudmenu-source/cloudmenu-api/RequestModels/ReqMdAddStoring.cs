using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdAddStoring : ReqMdBase
    {
        // 商品番号
        public string productNumber { get; set; }
        // 入庫日
        public string storingDate { get; set; }
        // 区分値
        public string itemKbn { get; set; }
        // 入庫数
        public short storingQuantity { get; set; }
        // 備考
        public string remarks { get; set; }
    }
}
