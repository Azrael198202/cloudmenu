using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       商品詳細のリスト
    *****/
    public class DbExtShohinDetailsReception
    {
        public string productNumber { get; set; }//商品番号
        public string setNumber { get; set; }//商品セット品番号
        public string image { get; set; }//商品画像
        public string nameCh { get; set; }//商品名称（中国語）
        public string nameJp { get; set; }//商品名称（日本語）
        public ushort? calorie { get; set; }//カロリー
        public string offertimeFrom { get; set; }//提供時間帯From
        public string offertimeTo { get; set; }//提供時間帯To
        public uint price { get; set; }//価格（税込）
        public string introduction { get; set; }//商品紹介
        public string pickupFlg { get; set; }//お薦めフラグ
        public string existenceFlg { get; set; }//セット品有無フラグ
        public string courseFlg { get; set; }//コース料理フラグ   
        public string preparationFlg { get; set; }//準備中フラグ
        public string remarks { get; set; }//商品備考   
        public string typeKbn { get; set; }//商品分類区分
    }
}
