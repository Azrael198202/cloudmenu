using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
#nullable disable

namespace cloudmenu_api.ResponseModels
{

    /*****
    *       商品詳細内容検索
    *****/
    public class RpnShohinDetails : RpnMdBase
    {
        public List<DbExtShohinDetailsReception> shohinDetailsList { get; set; } = new List<DbExtShohinDetailsReception>();//商品詳細のリスト
        public List<DbExtAllergiesList> allergiesList { get; set; } = new List<DbExtAllergiesList>();//商品アレルギー情報のリスト
        public List<DbExtStoreList> storeList { get; set; } = new List<DbExtStoreList>();//店舗情報リスト
        public List<DbExtCourseList> courseList { get; set; } = new List<DbExtCourseList>();//コース料理リスト
    }
}
