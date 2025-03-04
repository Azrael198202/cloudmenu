using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudmenu_api.dailyReport;
using cloudmenu_api.DbEntitiesCloudMenu;

namespace cloudmenu_api.DailyReport
{
    public class CancelPrintReport
    {
        public string StoreNumber{ get; set; }
        public string ReceptioNumber{ get; set; }
        public string ProductName{ get; set; }
        public string CancelFlg{ get; set; }
        public byte? OrderQuantity{ get; set; }
        public uint? ProductPrice{ get; set; }
        public uint? PaymentPrice{ get; set; }
        public uint? LastPaymentPrice{ get; set; }
        
    }
}