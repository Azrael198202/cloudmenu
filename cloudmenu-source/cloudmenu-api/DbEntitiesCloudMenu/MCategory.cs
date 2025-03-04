using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MCategory
    {
        public string StoreNumber { get; set; }
        public string CategoryClassNumber { get; set; }
        public string CategoryKbn { get; set; }
        public string CategoryKbnName { get; set; }
        public string CategoryKbnAbbreviation { get; set; }
        public string CategoryOptionValues { get; set; }
        public string DelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
