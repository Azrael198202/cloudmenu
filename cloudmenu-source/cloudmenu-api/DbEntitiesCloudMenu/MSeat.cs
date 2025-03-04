using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MSeat
    {
        public string StoreNumber { get; set; }
        public string SeatLevel { get; set; }
        public string SeatNumber { get; set; }
        public string SeatKbn { get; set; }
        public string SeatName { get; set; }
        public byte? SeatQuantity { get; set; }
        public ushort? SeatSizeVertical { get; set; }
        public ushort? SeatSizeHorizontal { get; set; }
        public ushort? SeatPositionVertical { get; set; }
        public ushort? SeatPositionHorizontal { get; set; }
        public string DelFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
