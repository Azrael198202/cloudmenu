using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{

    /*****
    *     区分マスタ
    *****/
    public class ReqKbnOrder : ReqMdBase
    {
        public string categoryClassNumber { get; set; }//区分分類コード
    }
}
