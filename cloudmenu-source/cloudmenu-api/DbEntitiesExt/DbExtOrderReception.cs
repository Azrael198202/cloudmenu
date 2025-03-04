using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       注文管理リスト
    *****/
    public class DbExtOrderReception
    {
        public string orderDatetime { get; set; }//注文日時
        public string orderVoucherNumber { get; set; }//注文伝票番号
        public string orderVoucherDetails { get; set; }//注文伝票明細番号
        public string orderProductNumber { get; set; }//商品番号
        public string orderProductSetNumber { get; set; }//商品セット番号
        public string orderProductNameCh { get; set; }//商品名称（中国語）
        public string orderProductSetName { get; set; }//商品セット名称
        public string orderProductTypeKbn { get; set; }//商品分類区分
        public uint orderQuantity { get; set; }//個数
        public string orderTakeoutFlg { get; set; }//持ち帰りフラグ
        public string orderRemarks { get; set; }//備考情報
        public string orderSlipFlg { get; set; }//伝票出力フラグ

        // public sbyte? ordernumberOfPeople { get; set; }//人数
    }
    //注文伝票番号を採番する
    public class DbExOrderNumReception
    {
        public string orderVoucherDetails { get; set; }//注文伝票明細番号
    }
}
