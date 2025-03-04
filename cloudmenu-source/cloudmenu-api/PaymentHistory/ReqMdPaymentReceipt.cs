using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.RequestModels;

#nullable disable

namespace cloudmenu_api.PaymentHistory
{
    public class ReqMdPaymentReceipt : ReqMdBase
    {
        public string receptionNumber { get; set; }
        public string paymentNumber { get; set; }
        public bool withReceipt { get; set; }
        //領収書出力フラグ
        public bool withReceiptChit { get; set; }

    }
}
