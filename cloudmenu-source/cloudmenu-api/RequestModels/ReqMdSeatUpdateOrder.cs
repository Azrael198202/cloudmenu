using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.RequestModels
{
    public class ReqMdSeatUpdateOrder : ReqMdBase
    {
        public string ReceptionNumberToDel { get; set; }
        public string ReceptionNumber { get; set; }
        public ushort ReceptionBranchNumber { get; set; }
        public string OrderVoucherNumber { get; set; }
        public string OrderVoucherDetails { get; set; }
        public byte? OrderQuantity { get; set; }
        public uint? OrderDiscountYen { get; set; }
        public string Remark { get; set; }
    }
}
