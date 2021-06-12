using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class GetSystemMonitorRecordsModel
    {
        public String StartDate { get; set; } = "";
        public String EndDate { get; set; } = "";
        public string AdminID { get; set; } = "";
    }
}
