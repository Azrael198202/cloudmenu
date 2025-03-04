using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *      v_product_menu_item
    *****/
    public class DbExtBChoiceReception
    {
        public byte menuLineNumber { get; set; }//商品メニュー分類表示順
        public string menuOrderNumber { get; set; }//商品メニュー注文番号
        public string number { get; set; }//商品番号
        public string setNumber { get; set; }//商品セット番号
        public string nameCh { get; set; }//商品名称（中国語）
        public string nameJp { get; set; }//商品名称（日本語）
        public string setName { get; set; }//商品セット名称
        public int? price { get; set; }//商品番号
        public string taxKbn { get; set; }//税率区分
        public string offertimeFrom { get; set; }//提供時間帯From
        public string offertimeTo { get; set; }//提供時間帯To
        public byte? calorie { get; set; }//カロリー
        public byte? cookingtime { get; set; }//調理目安時間
        public string pickupFlg { get; set; }//お薦めフラグ
        public string setExistenceFlg { get; set; }//セット品有無フラグ
        public string limitedKbn { get; set; }//数量限定区分
        public string image { get; set; }//商品画像
        public string preparationFlg { get; set; }//準備中フラグ
        public string remarks { get; set; }//商品備考
        public byte? limitedQuantity { get; set; }//注文可能数
        public string typeKbn { get; set; }//商品分類区分
    }
}
