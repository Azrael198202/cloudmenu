using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudmenu_api.dailyReport;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbEntitiesExt;

namespace cloudmenu_api.DailyReport
{
    public class CancelReport
    {
        public DateTime drDateTimeS { get; set; }

        public DateTime drDateTimeE { get; set; }

        public string drDateStrJpS { get; set; }
        
        public string drDateStrJpE { get; set; }

        public MStoreInfo drStoreInfo { get; set; }

        public List<DBTCancelOrder> CancelOrder = new List<DBTCancelOrder>();

        public List<CancelPrintReport> LstCancelOrder = new List<CancelPrintReport>();
    }
}