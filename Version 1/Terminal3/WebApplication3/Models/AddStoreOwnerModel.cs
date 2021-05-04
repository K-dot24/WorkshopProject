using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class AddStoreOwnerModel
    {
        public string addedOwnerID { get; set; } = "";
        public string currentlyOwnerID { get; set; } = "";
        public string storeID { get; set; } = "";
    }
}
