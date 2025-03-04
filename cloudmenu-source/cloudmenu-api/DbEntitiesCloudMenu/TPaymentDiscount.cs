using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TPaymentDiscount
    {
        public string StoreNumber { get; set; }
        public string PaymentNumber { get; set; }
        public ushort PaymentBranchNumber { get; set; }
        public string DiscountPremiumKbn { get; set; }
        public byte? DiscountPercent { get; set; }
        public uint? DiscountYen { get; set; }
        public byte? PremiumPercent { get; set; }
        public uint? PremiumYen { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
