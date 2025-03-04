using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;

namespace cloudmenu_api.dailyReport
{

    /*****
    * 日計レポート出力用データクラス
    *****/
    public class DailyReportData
    {
        public DateTime drDateTimeS { get; set; }
        public DateTime drDateTimeE { get; set; }
        public string drDateStrJpS { get; set; }
        public string drDateStrJpE { get; set; }
        public MStoreInfo drStoreInfo { get; set; }
        public List<MCategoryClass> drPaymentClassKbns = new List<MCategoryClass>();
        public List<MCategory> drPaymentKbns = new List<MCategory>();
        public List<DRReceiption> drReceiptions { get; set; } = new List<DRReceiption>();
        public List<DROrder> drOrders { get; set; } = new List<DROrder>();
        public List<DRPayment> drPayments = new List<DRPayment>();
        public List<DRPaymentDiscount> drPaymentDiscounts = new List<DRPaymentDiscount>();
    }
}
