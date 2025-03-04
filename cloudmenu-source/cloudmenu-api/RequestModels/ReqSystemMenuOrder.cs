using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *       システムメニューマスタ
    *****/
    public class ReqSystemMenuOrder : ReqMdBase
    {
        public string menuUserKbn { get; set; }//利用者区分
    }
}
