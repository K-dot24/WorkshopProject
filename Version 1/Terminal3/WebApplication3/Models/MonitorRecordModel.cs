using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class MonitorRecordModel
    {
        public string Date { get; set; }= "";
        public int GuestUsers { get; set; } = 0;
        public int RegisteredUsers { get; set; } = 0;
        public int ManagersNotOwners { get; set; } = 0;
        public int Owners { get; set; } = 0;
        public int Admins { get; set; } = 0;
    }
}
