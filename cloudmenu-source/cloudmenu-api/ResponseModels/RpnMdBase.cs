using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *ステータス
    *****/
    public class RpnMdBase
    {
        // ステータス 
        public RepMdStatus status { get; set; } = RepMdStatus.S_200_OK;
        // メッセージのリスト
        public RpnMdMsg msgList { get; set; } = new RpnMdMsg();
    }
}
