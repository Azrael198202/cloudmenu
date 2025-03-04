using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TProductStoringIssue
    {
        public string StoreNumber { get; set; }
        public string ProductNumber { get; set; }
        public string ProductStoringIssueDate { get; set; }
        public byte ProductStoringIssueTimes { get; set; }
        public string ProductStoringIssueKbn { get; set; }
        public short? ProductStoringIssueQuantity { get; set; }
        public string ProductStoringIssueRemarks { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
