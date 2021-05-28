using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ServiceLayer.Controllers
{
    public class GetIncomeAmountGroupByDayModel
    {
        public String StartDate { get; set; } = "";
        public String EndDate { get; set; } = "";
        public String StoreID { get; set; } = "";
        public String OwnerID { get; set; } = "";
    }
}
