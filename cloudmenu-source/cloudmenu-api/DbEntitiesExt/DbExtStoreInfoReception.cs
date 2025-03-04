using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.DbentitiesExt
{

    /*****
    *       店舗情報のリスト
    *****/
    public class DbExtStoreInfoReception
    {
        public string Name { get; set; }//店舗名
        public string Address { get; set; }//住所
        public string Tel { get; set; }//電話番号
        public string Url { get; set; }//URL
        public string WeekdaysFrom { get; set; }//営業時間平日From
        public string WeekdaysTo { get; set; }//営業時間平日To
        public string SaturdayFrom { get; set; }//営業時間土曜日From
        public string SaturdayTo { get; set; }//営業時間土曜日To
        public string HolidaysFrom { get; set; }//営業時間日祝From
        public string HolidaysTo { get; set; }//営業時間日祝To
        public byte? SeatQuantity { get; set; }//座席数
        public string AllergiesDisplayFlg { get; set; }//アレルギー表示有無フラグ
        public string LogoImage { get; set; }//店舗ロゴ
        public string Image { get; set; }//店舗写真
        public string Introduction { get; set; }//店舗紹介
        public string StaffWord { get; set; }//一言
    }
}
