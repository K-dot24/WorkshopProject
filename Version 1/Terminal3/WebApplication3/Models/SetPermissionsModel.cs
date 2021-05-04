using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class SetPermissionsModel
    {
        public String storeID { get; set; } = "";
        public String managerID { get; set; } = "";
        public String ownerID { get; set; } = "";
        public LinkedList<int> permissions { get; set; } = null;
    }
}
