using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudmenu_api.DbEntitiesExt
{
    public class DBTCancelOrder
    {
        public string ReceptioNumber{ get; set; }
        public string CancelUser{ get; set; }

        public DateTime? CancelDate{ get; set; }
    }
}