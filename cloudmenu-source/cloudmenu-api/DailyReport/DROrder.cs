using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.dailyReport
{

    /*****
    * 日計レポート出力用データクラス
    *****/
    public class DROrder
    {
        public string StoreNumber { get; set; }
        public string ReceptionNumber { get; set; }
        public ushort ReceptionBranchNumber { get; set; }
        public string OrderVoucherNumber { get; set; }
        public string OrderVoucherDetails { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        // public string ProductSetNumber { get; set; }
        // public string ProductNameCh { get; set; }
        // public string ProductSetName { get; set; }
        public string ProductTypeKbn { get; set; }
        // public uint? ProductPrice { get; set; }
        public byte? OrderQuantity { get; set; }
        // public uint? OrderPrice { get; set; }
        // public byte? OrderDiscountPercent { get; set; }
        public uint? OrderDiscountYen { get; set; }
        public uint? OrderAfterPrice { get; set; }
        public string OrderTakeoutFlg { get; set; }
        // public string OrderCookingFlg { get; set; }
        // public string OrderServeFlg { get; set; }
        // public string OrderCancelFlg { get; set; }
        // public string OrderRemarks { get; set; }
        public string PaymentNumber { get; set; }
        public int taxRate { get; set; }
        public long taxAmount { get; set; }
    }
}
