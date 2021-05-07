using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class AddProductToStoreModel
    {
        public String userID { get; set; } = "";
        public String storeID { get; set; } = "";
        public String productName { get; set; } = "";
        public double price { get; set; } = 0;
        public int initialQuantity { get; set; } = 0;
        public String category { get; set; } = "";
        public LinkedList<String> keywords { get; set; } = null;
    }
}
