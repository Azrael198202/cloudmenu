using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudmenu_api.DbEntitiesExt
{
    public class DBCancel
    {
        public string strReceptioNumber{ get; set; }

        public string strProductName{ get; set; }

        public string strCancelFlg{ get; set; }

        public string strOrderPrice{ get; set; }

        public string strProductPrice{ get; set; }

        public string strPaymentPrice{ get; set; }

        public string strLastPaymentPrice{ get; set; }
    }
}