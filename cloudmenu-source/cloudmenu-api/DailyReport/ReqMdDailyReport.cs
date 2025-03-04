using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.RequestModels;

#nullable disable

namespace cloudmenu_api.dailyReport
{
    public class ReqMdDailyReport : ReqMdBase
    {
        public bool reportIsMonth { get; set; }
        public string reportDate { get; set; }
        public List<string> reportTypes { get; set; }
    }
}
