using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdCashierPrint : ReqMdBase
    {
        public MStoreInfo storeInfo { get; set; } = new MStoreInfo();
        //受付情報
        public DbExtSeatSumInfos seatTakeoutRecpInfo { get; set; } = new DbExtSeatSumInfos();
        //受付の注文一覧
        public List<DbExtOrderDetail> orderInfoList { get; set; } = new List<DbExtOrderDetail>();

        //会計種類
        public string paymentClassKbn { get; set; }
        public string paymentKbn { get; set; }
        //割引区分
        public int discountType { get; set; }
        public int discountPercent { get; set; }
        //割引額
        public int discountAmount { get; set; }
        public int serviceAmount { get; set; }
        //レシート出力フラグ 2021/07/06 追加
        public bool withReceipt { get; set; }
        //領収書出力フラグ
        public bool withReceiptChit { get; set; }
    }
}
