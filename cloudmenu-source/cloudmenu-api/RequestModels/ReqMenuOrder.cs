using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       商品メニューマスタ
    *****/
    public class ReqMenuOrder : ReqMdBase
    {
        public string menuNovisibleFlg { get; set; }//非表示取得
    }
}
