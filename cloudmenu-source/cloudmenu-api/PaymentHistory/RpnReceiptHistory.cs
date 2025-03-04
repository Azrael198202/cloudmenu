using System.Collections.Generic;
using cloudmenu_api.ResponseModels;

#nullable disable

namespace cloudmenu_api.PaymentHistory
{
    public class RpnReceiptHistory : RpnMdBase
    {
        public List<ReceiptHisData> receiptionHisList { get; set; } = new List<ReceiptHisData>();
    }
}
