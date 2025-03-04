using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       v_product_menu_item
    *****/
    public class ReqSignature : ReqMdBase
    {
        public string receptionNumber { get; set; } //受付番号
        public string signatureJpgStr { get; set; } //署名画像
    }
}
