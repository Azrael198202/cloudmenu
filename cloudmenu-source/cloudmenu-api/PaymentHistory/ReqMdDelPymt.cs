using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.RequestModels;

#nullable disable

namespace cloudmenu_api.PaymentHistory
{
    public class ReqMdDelPymt : ReqMdBase
    {
        public List<string> paymentHisList { get; set; }
    }
}
