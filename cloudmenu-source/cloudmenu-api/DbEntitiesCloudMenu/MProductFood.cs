using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MProductFood
    {
        public string StoreNumber { get; set; }
        public string ProductNumber { get; set; }
        public string ProductNameCh { get; set; }
        public string ProductNameJp { get; set; }
        public string ProductOffertimeFrom { get; set; }
        public string ProductOffertimeTo { get; set; }
        public ushort? ProductCalorie { get; set; }
        public ushort? ProductCookingtime { get; set; }
        public string ProductPicupFlg { get; set; }
        public string ProductStokFlg { get; set; }
        public string ProductLimitedKbn { get; set; }
        public string ProductSetExistenceFlg { get; set; }
        public string ProductTypeKbn { get; set; }
        public string ProductCourseFlg { get; set; }
        public byte? ProductCourseOrder { get; set; }
        public string ProductUnitKbn { get; set; }
        public string ProductImage { get; set; }
        public string ProductIntroduction { get; set; }
        public string ProductPreparationFlg { get; set; }
        public string ProductRemarks { get; set; }
        public string DelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
