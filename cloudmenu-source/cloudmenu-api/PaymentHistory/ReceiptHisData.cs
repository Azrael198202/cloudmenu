using System;
using System.Collections.Generic;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.RequestModels;

#nullable disable

namespace cloudmenu_api.PaymentHistory
{
    public class ReceiptHisData
    {
        public string StoreNumber { get; set; }
        public string ReceptionNumber { get; set; }
        public ushort ReceptionBranchNumber { get; set; }
        public string ReceptionKbn { get; set; }
        public string SeatNumber { get; set; }
        public string SeatName { get; set; }
        public int SeatPeopleAll { get; set; }

        //--------------------------------------------------
        // public string SeatNumbers { get; set; }
        public string SeatNames { get; set; }
        public int paymentStatus { get; set; }
        public string paymentStatusName { get; set; }

        //--------------------------------------------------
        // 会計情報一覧
        public List<PaymentData> paymentHisList { get; set; } = new List<PaymentData>();

    }
}
