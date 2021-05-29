using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class PolicyModel
    {
        public string storeId { get; set; } = "";
        public Dictionary<string, object> info { get; set; } = null;
    }
}
