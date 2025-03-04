using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MStoreInfo
    {
        public string StoreNumber { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreAddress2 { get; set; }
        public string StoreTel { get; set; }
        public string StoreUrl { get; set; }
        public string StoreWeekdaysFrom { get; set; }
        public string StoreWeekdaysTo { get; set; }
        public string StoreSaturdayFrom { get; set; }
        public string StoreSaturdayTo { get; set; }
        public string StoreHolidaysFrom { get; set; }
        public string StoreHolidaysTo { get; set; }
        public byte? StoreSeatQuantity { get; set; }
        public string StoreAllergiesDisplayFlg { get; set; }
        public string StoreLogoImage { get; set; }
        public string StoreImage { get; set; }
        public string StoreIntroduction { get; set; }
        public string StoreStaffWord { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
