using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class VProductMenuItem
    {
        public string StoreNumber { get; set; }
        public string ProductMenuNumber { get; set; }
        public byte ProductMenuLineNumber { get; set; }
        public string ProductMenuClassKbn { get; set; }
        public string MenuClassKbnName { get; set; }
        public string MenuClassKbnAbbr { get; set; }
        public string ProductMenuNovisibleFlg { get; set; }
        public string ProductMenuOrderNumber { get; set; }
        public string ProductNumber { get; set; }
        public string ProductSetNumber { get; set; }
        public string ProductNameCh { get; set; }
        public string ProductNameJp { get; set; }
        public string ProductSetName { get; set; }
        public uint ProductPrice { get; set; }
        public string ProductTaxKbn { get; set; }
        public string ProductOffertimeFrom { get; set; }
        public string ProductOffertimeTo { get; set; }
        public ushort? ProductCalorie { get; set; }
        public ushort? ProductCookingtime { get; set; }
        public string ProductPicupFlg { get; set; }
        public string ProductStokFlg { get; set; }
        public string ProductSetExistenceFlg { get; set; }
        public string ProductLimitedKbn { get; set; }
        public string ProductTypeKbn { get; set; }
        public string TypeKbnName { get; set; }
        public string TypeKbnAbbr { get; set; }
        public string ProductCourseFlg { get; set; }
        public byte? ProductCourseOrder { get; set; }
        public string ProductUnitKbn { get; set; }
        public string UnitKbnName { get; set; }
        public string UnitKbnAbbr { get; set; }
        public string ProductImage { get; set; }
        public string ProductIntroduction { get; set; }
        public string ProductPreparationFlg { get; set; }
        public string ProductRemarks { get; set; }
        public decimal? LimitedQuantity { get; set; }
    }
}
