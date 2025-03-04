using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TMaterialStoringIssue
    {
        public string StoreNumber { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialStoringIssueDate { get; set; }
        public byte MaterialStoringIssueTimes { get; set; }
        public string MaterialStoringIssueKbn { get; set; }
        public short? MaterialStoringIssueQuantity { get; set; }
        public string MaterialStoringIssueRemarks { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
