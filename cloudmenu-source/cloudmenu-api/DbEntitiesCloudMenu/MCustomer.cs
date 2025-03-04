using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MCustomer
    {
        public string StoreNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerKbn { get; set; }
        public string CustomerName { get; set; }
        public string CustomerConnection { get; set; }
        public string CustomerTel { get; set; }
        public string CustomerZip { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerBirthday { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerSex { get; set; }
        public string CustomerRemarks { get; set; }
        public string CustomerDelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
