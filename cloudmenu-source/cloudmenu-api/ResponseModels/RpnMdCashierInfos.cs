using System.Linq;
using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.LwUtils;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    * 座席最新データ検索
    *****/
    public class RpnMdCashierInfos : RpnMdBase
    {
        public MStoreInfo storeInfo { get; set; } = new MStoreInfo();
        //受付情報
        public DbExtSeatSumInfos seatTakeoutRecpInfo { get; set; } = new DbExtSeatSumInfos();
        //受付の注文一覧
        public List<DbExtOrderDetail> orderInfoList { get; set; } = new List<DbExtOrderDetail>();

        public List<MCategoryClass> paymentGpClassLst { get; set; } = new List<MCategoryClass>();
        public List<MCategory> paymentClassLst { get; set; } = new List<MCategory>();
    }
}
