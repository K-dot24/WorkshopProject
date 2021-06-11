using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class GetSystemMonitorRecordsModel
    {
        public String start_date { get; set; } = "";
        public String end_date { get; set; } = "";
        public string admin_id { get; set; } = "";
    }
}
