using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MDiscountPremium
    {
        public string StoreNumber { get; set; }
        public string DiscountPremiumKbn { get; set; }
        public string DiscountPremiumName { get; set; }
        public byte? DiscountPercent { get; set; }
        public byte? DiscountYen { get; set; }
        public byte? PremiumPercent { get; set; }
        public byte? PremiumYen { get; set; }
        public string DiscountPremiumDelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
