using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountTargetShop : IDiscountTarget
    {

        public static Result<IDiscountTarget> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountTarget>("", true, new DiscountTargetShop());
        }

        public List<Product> getTargets(ConcurrentDictionary<Product, int> products)
        {
            List<Product> result = new List<Product>();
            foreach (KeyValuePair<Product, int> entry in products)
                result.Add(entry.Key);
            return result;
        }

        public Result<String> GetData()
        {
            return new Result<string>("", true, "the whole store");
        }

        public string getId()
        {
            return "";
        }
    }
}
