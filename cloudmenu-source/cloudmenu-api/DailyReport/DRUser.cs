using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudmenu_api.dailyReport;
using cloudmenu_api.DbEntitiesCloudMenu;

namespace cloudmenu_api.DailyReport
{
    public class DRUser
    {
        public string StoreNumber { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
    }
}