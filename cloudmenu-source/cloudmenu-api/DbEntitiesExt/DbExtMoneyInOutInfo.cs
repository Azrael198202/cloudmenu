using System;
using cloudmenu_api.LwUtils;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *プリンター伝票データ
    *****/
    public class DbExtMoneyInOutInfo
    {
        public string StoreNumber { get; set; }
        public string MoneyInoutNumber { get; set; }
        public DateTime MoneyInoutDatetime { get; set; }
        public uint? MoneyInoutPrice { get; set; }
        public string MoneyInoutKbn { get; set; }
        public string MoneyInoutReasonKbn { get; set; }
        public string MoneyInoutRbFlg { get; set; }
        public string MoneyInoutOriginNumber { get; set; }
        public string MoneyInoutRemarks { get; set; }

        //------------------------------------------------------------
        public string MoneyInoutDateStr => MoneyInoutDatetime.ToString("yyyy/MM/dd");
        public string moneyInoutKbnName { get; set; }
        public string MoneyInoutReasonKbnName { get; set; }
    }
}
