using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class ProductToCart
    {
        public String userID { get; set; } = "";
        public String ProductID { get; set; } = "";
        public int ProductQuantity { get; set; } = 0;
        public String StoreID { get; set; } = "";
    }
}
