using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       v_product_menu_item
    *****/
    public class ReqChoiceOrder : ReqMdBase
    {
        public byte menuLineNumber { get; set; }//商品メニュー分類表示順
        public string menuOrderNumber { get; set; }//商品メニュー注文番号
    }
}
