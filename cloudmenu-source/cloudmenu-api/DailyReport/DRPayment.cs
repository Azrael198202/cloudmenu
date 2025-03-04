using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.dailyReport
{

    /*****
    * 日計レポート出力用データクラス
    *****/
    public class DRPayment
    {
        public string StoreNumber { get; set; }
        public string PaymentNumber { get; set; }
        public DateTime PaymentDatetime { get; set; }
        public uint? PaymentPrice { get; set; }
        public int? PaymentConfirmPrice { get; set; }
        public string PaymentClassKbn { get; set; }
        public string PaymentKbn { get; set; }

        public long PaymentPriceTax { get; set; }
        // public string PaymentRbFlg { get; set; }
        // public string PaymentOriginNumber { get; set; }
        // public string PaymentRemarks { get; set; }
    }
}
