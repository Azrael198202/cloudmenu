using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *          コース料理リスト
    *****/
    public class DbExtCourseList
    {
        public string courseProductName { get; set; }// コース料理名
        public byte? courseOrder { get; set; }// コース料理順
    }
}
