using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       注文履歴のリスト
    *****/
    public class DbHistoryOrderReception
    {
        public DateTime orderDatetime { get; set; }//注文日時
        public string orderVoucherNumber { get; set; }//注文伝票番号
        public string orderVoucherDetails { get; set; }//注文伝票明細番号
        public string orderProductNumber { get; set; }//商品番号
        public string orderProductSetNumber { get; set; }//商品セット番号
        public string orderProductNameCh { get; set; }//商品名称（中国語）
        public string orderProductNameJp { get; set; }//商品名称（日本語）
        public string orderProductSetName { get; set; }//商品セット名称
        public string orderProductTypeKbn { get; set; }//商品分類区分   
        public byte? orderQuantity { get; set; }//個数
        public uint? orderPrice { get; set; }//価格
        public uint? orderDiscountPrice { get; set; }//値引き後価格
        public string orderTakeoutFlg { get; set; }//持ち帰りフラグ
        public string orderCookingFlg { get; set; }//調理指示フラグ
        public string orderServeFlg { get; set; }//配膳完了フラグ
        public string orderRemarks { get; set; }//備考情報
    }
}
