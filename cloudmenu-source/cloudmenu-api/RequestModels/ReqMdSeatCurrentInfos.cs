using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdSeatCurrentInfos : ReqMdBase
    {
        //選択された受付番号
        public string SelectedRecpNumber { get; set; }
    }
}
