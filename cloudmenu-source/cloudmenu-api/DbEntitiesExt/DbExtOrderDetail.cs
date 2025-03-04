using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.LwUtils;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       注文管理リスト
    *****/
    public class DbExtOrderDetail
    {
        public string StoreNumber { get; set; }//店舗番号
        public string ReceptionNumber { get; set; }//受付番号
        public ushort ReceptionBranchNumber { get; set; }//受付枝番
        public string SeatName { get; set; }//座席名
        public string OrderVoucherNumber { get; set; }//注文伝票番号
        public string OrderVoucherDetails { get; set; }//注文伝票明細番号
        public DateTime OrderDate { get; set; }//注文日時
        public string ProductNumber { get; set; }//商品番号
        public string ProductSetNumber { get; set; }//商品セット番号
        public string ProductNameCh { get; set; }//商品名称（中国語）
        public string ProductSetName { get; set; }//商品セット名称
        public string ProductTypeKbn { get; set; }//商品分類区分
        public uint? ProductPrice { get; set; }//価格（税込）
        public byte? OrderQuantity { get; set; }//個数
        public uint? OrderPrice { get; set; }//金額
        public byte? OrderDiscountPercent { get; set; }//割引
        public uint? OrderDiscountYen { get; set; }//値引き
        public uint? OrderAfterPrice { get; set; }//値割引後金額
        public string OrderTakeoutFlg { get; set; }//持帰りフラグ
        public string OrderCookingFlg { get; set; }//調理指示フラグ
        public string OrderServeFlg { get; set; }//配膳完了フラグ
        public string OrderCancelFlg { get; set; }//取消フラグ
        public string OrderRemarks { get; set; }//備考
        public string PaymentNumber { get; set; }//会計番号
        public DateTime? RegistrationDatetime { get; set; }//登録日時

        //extra add
        public string ProductNameJp { get; set; }//商品名称（日本語）
        public string ProductTypeName { get; set; }//商品分類名称
        public int taxRate { get; set; } = 0;//費税率
        public long includedTaxAmount { get; set; } = 0;//内消費税
        //会計レジ画面用
        public int paymentOver { get; set; } = 1; //会計済みフラグ 0:未 1:済
        public int casherPaySelected { get; set; } = 0; //会計選択フラグ 0:未 1:済
        // computed
        // 持ち帰り名称
        public string takeoutName => this.OrderTakeoutFlg == FlgConst.on ? "持帰" : "";
        // public string productShowName => String.IsNullOrEmpty(ProductSetName) ? ProductNameCh : ProductSetName;
        public string productShowName => ProductNameCh;
        // public string productShowNameJp => String.IsNullOrEmpty(ProductSetName) ? ProductNameJp : "";
        public string productShowNameJp => ProductNameJp;
        public string timePassedSinceOrder => OrderServeFlg == FlgConst.on ? "" : (DateTime.Now.Subtract(OrderDate).ToString(@"hh\:mm"));
        public string orderCookingState => OrderCookingFlg == FlgConst.on ? "済" : "未";
        public string orderServeState => OrderServeFlg == FlgConst.on ? "済" : "未";
        public string orderDateMMdd => OrderDate.ToString("MM月dd日");
        public string orderDateHHmm => OrderDate.ToString("HH時mm分");
    }
}
