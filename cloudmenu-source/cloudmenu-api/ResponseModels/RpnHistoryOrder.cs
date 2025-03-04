using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *     注文履歴検索
    *****/
    public class RpnHistoryOrder : RpnMdBase
    {
        public List<DbHistoryOrderReception> orderHistoryList { get; set; } = new List<DbHistoryOrderReception>();//注文履歴のリスト
        public int orderQuantityTotal { get; set; }//合計個数
        public uint? orderPriceTotal { get; set; }//合計金額
    }
}
