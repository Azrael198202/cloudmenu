using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       
    *****/
    public class ReqSaveTana : ReqMdBase
    {
        // 棚卸日
        public string inventoryDate { get; set; }
        // 品目区分
        public string itemKbn { get; set; }
        // 分類区分
        public string categoryKbn { get; set; }
        // 棚卸一覧
        public List<TanaList> tanaList { get; set; }
        // 内訳一覧
        public List<UchiwakeList> uchiwakeList { get; set; }
    }
    /*****
    *       棚卸一覧リスト
    *****/
    public class TanaList
    {
        // 対象番号
        public string targetNumber { get; set; }
        // 対象名称
        public string targetName { get; set; }
        // 棚卸数
        public short? inventoryQuantity { get; set; }

    }
    /*****
    *       内訳一覧リスト
    *****/
    public class UchiwakeList
    {
        // 対象番号
        public string targetNumber { get; set; }
        // 対象名称
        public string targetName { get; set; }
        // 入出庫・消費フラグ
        public string targetFlg { get; set; }
        // 区分
        public string breakdownKbn { get; set; }
        // 内訳数
        public short? breakdownQuantity { get; set; }

    }
}
