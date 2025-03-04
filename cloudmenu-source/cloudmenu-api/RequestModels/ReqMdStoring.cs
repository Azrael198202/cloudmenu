using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdStoring : ReqMdBase
    {
        // 入庫日
        public string storingDate { get; set; }

        // 入庫種類区分
        public string itemKbn { get; set; }
    }
}
