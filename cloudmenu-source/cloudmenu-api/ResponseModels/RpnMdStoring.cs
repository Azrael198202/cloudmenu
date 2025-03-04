using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *
    *****/
    public class RpnMdStoring : RpnMdBase
    {
        public List<DbExtStoring> searchList { get; set; } = new List<DbExtStoring>();
    }
}
