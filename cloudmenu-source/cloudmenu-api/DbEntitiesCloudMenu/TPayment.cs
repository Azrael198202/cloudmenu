using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TPayment
    {
        public string StoreNumber { get; set; }
        public string PaymentNumber { get; set; }
        public DateTime PaymentDatetime { get; set; }
        public uint? PaymentPrice { get; set; }
        public int? PaymentConfirmPrice { get; set; }
        public string PaymentClassKbn { get; set; }
        public string PaymentKbn { get; set; }
        public string PaymentRbFlg { get; set; }
        public string PaymentOriginNumber { get; set; }
        public string PaymentRemarks { get; set; }
        public string ClosingFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
