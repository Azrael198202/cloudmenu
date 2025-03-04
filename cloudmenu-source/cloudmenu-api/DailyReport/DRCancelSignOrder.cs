using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudmenu_api.dailyReport;
using cloudmenu_api.DbEntitiesCloudMenu;

namespace cloudmenu_api.DailyReport
{
    public class DRCancelSignOrder
    {
        public string StoreNumber { get; set; }
        public string ReceptionNumber { get; set; }
        public ushort ReceptionBranchNumber { get; set; }
        public string UserName { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}