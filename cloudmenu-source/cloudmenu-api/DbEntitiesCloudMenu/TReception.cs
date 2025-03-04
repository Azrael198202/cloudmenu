using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class TReception
    {
        public string StoreNumber { get; set; }
        public string ReceptionNumber { get; set; }
        public ushort ReceptionBranchNumber { get; set; }
        public string ReceptionKbn { get; set; }
        public string SeatLevel { get; set; }
        public string SeatNumber { get; set; }
        public DateTime? SeatStartDate { get; set; }
        public DateTime? SeatEndDate { get; set; }
        public string SeatStatusKbn { get; set; }
        public byte? SeatPeopleMan { get; set; }
        public byte? SeatPeopleWoman { get; set; }
        public byte? SeatPeopleChildren { get; set; }
        public string CustomerNumber { get; set; }
        public string TakeoutName { get; set; }
        public string TakeoutTel { get; set; }
        public DateTime? TakeoutReceiptTime { get; set; }
        public string ReceptionRemarks { get; set; }
        public string ReceptionDelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
