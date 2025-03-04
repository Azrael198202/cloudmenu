using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.DbEntitiesCloudMenu
{
    public partial class MSystemMenu
    {
        public string StoreNumber { get; set; }
        public string SystemMenuUserKbn { get; set; }
        public byte SystemMenuYPosition { get; set; }
        public byte SystemMenuXPosition { get; set; }
        public string SystemMenuId { get; set; }
        public string SystemMenuName { get; set; }
        public string SystemMenuLink { get; set; }
        public string SystemMenuButtonColor { get; set; }
        public string SystemMenuButtonIcon { get; set; }
        public string SystemMenuFontColor { get; set; }
        public string SystemMenuUnusedFlg { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime? RegistrationDatetime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
