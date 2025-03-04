using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
#nullable disable

namespace cloudmenu_api.dailyReport
{

    /*****
    * 日計レポート出力用データクラス
    *****/
    public class DRReceiption
    {
        public string StoreNumber { get; set; }
        public string ReceptionNumber { get; set; }
        public ushort ReceptionBranchNumber { get; set; }
        public string ReceptionKbn { get; set; }
        public DateTime? SeatStartDate { get; set; }
        public DateTime? SeatEndDate { get; set; }
        public byte? SeatPeopleMan { get; set; }
        public byte? SeatPeopleWoman { get; set; }
        public byte? SeatPeopleChildren { get; set; }

        public string SeatName { get; set; }
        // public int taxRate { get; set; }
    }
}
