using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class PurchaseModel
    {
        public String userID { get; set; } = "";
        public IDictionary<String, Object> paymentDetails { get; set; }
        public IDictionary<String, Object> deliveryDetails { get; set; }
    }
}
