using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       注文データ登録
    *****/
    public class ReqInsertOrder : ReqMdBase
    {
        // public string storeNumber { get; set; }//商品メニュー分類表示順
        public string receptionNumber { get; set; }//受付番号
        public ushort receptionBranchNumber { get; set; }//受付枝番
        public string seatLevel { get; set; }//階層
        public string seatNumber { get; set; }//座席番号
        public List<OrderCart> orderCart { get; set; }//受付番号
    }
    /*
    **  注文カート領域
    */
    public class OrderCart
    {
        public string orderDatetime { get; set; }//システム日時
        public string orderNumber { get; set; }//明細番号
        public string productNumber { get; set; }//商品番号
        public string productSetNumber { get; set; }//商品セット番号
        public string productNameCh { get; set; }//商品名称（中国語）
        public string productSetName { get; set; }//商品セット名称
        public string productTypeKbn { get; set; }//商品分類区分
        public uint orderQuantity { get; set; }//個数
        public uint orderPrice { get; set; }//価格
        public string orderTakeoutFlg { get; set; }//持帰りフラグ
        public string orderRemarks { get; set; }//備考
        public string orderSlipFlg { get; set; }//伝票出力フラグ
    }
}
