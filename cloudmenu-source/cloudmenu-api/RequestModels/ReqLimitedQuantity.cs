using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       注文可能数変更一時領域
    *****/
    public class ReqLimitedQuantity : ReqMdBase
    {
        // 選択商品分類リスト
        public List<SelectedTypeKbnList> selectedTypeKbnList { get; set; }
    }
    /*****
    *       選択商品分類リスト
    *****/
    public class SelectedTypeKbnList
    {
        // 商品分類区分名称
        public string productTypeKbnName { get; set; }
        // 商品分類区分値
        public string productTypeKbn { get; set; }
        // 商品リスト
        public List<ProductList> productList { get; set; }
    }
    /*****
     *       選択商品分類リスト
     *****/
    public class ProductList
    {
        // 商品番号
        public string productNumber { get; set; }
        // 商品セット番号
        public string productSetNumber { get; set; }
        // 数量限定区分スイッチ
        public string productLimitedKbnSw { get; set; }
        // 注文可能数
        public byte? productLimitedQuantity { get; set; }
    }
}
