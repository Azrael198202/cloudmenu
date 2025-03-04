using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TEquipmentStoringIssue
    {
        public string StoreNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string EquipmentStoringIssueDate { get; set; }
        public byte EquipmentStoringIssueTimes { get; set; }
        public string EquipmentStoringIssueKbn { get; set; }
        public short? EquipmentStoringIssueQuantity { get; set; }
        public string EquipmentStoringIssueRemarks { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
