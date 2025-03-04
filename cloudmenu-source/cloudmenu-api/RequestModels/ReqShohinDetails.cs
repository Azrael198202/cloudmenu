using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       商品詳細内容検索
    *****/
    public class ReqShohinDetails : ReqMdBase
    {
        public string productNumber { get; set; }//商品番号
    }
}
