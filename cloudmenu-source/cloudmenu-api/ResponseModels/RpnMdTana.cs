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
    public class RpnMdTana : RpnMdBase
    {
        public List<DbExtInventory> searchList { get; set; } = new List<DbExtInventory>();
    }
}
