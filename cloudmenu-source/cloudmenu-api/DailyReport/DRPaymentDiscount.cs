using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.dailyReport
{

    /*****
    * 日計レポート出力用データクラス
    *****/
    public class DRPaymentDiscount
    {
        public string StoreNumber { get; set; }
        public string PaymentNumber { get; set; }
        public ushort PaymentBranchNumber { get; set; }
        public string DiscountPremiumKbn { get; set; }
        public byte? DiscountPercent { get; set; }
        public uint? DiscountYen { get; set; }
        public long DiscountTax { get; set; }
        public byte? PremiumPercent { get; set; }
        public uint? PremiumYen { get; set; }
        public long PremiumTax { get; set; }
    }
}
