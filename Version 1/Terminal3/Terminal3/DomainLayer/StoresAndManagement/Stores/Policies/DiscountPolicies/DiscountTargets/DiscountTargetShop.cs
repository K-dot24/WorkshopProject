using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountTargetShop : IDiscountTarget
    {

        public List<Product> getTargets(ConcurrentDictionary<Product, int> products)
        {
            List<Product> result = new List<Product>();
            foreach (KeyValuePair<Product, int> entry in products)
                result.Add(entry.Key);
            return result;
        }

        public Result<IDiscountTargetData> GetData()
        {
            return new Result<IDiscountTargetData>("", true, new DiscountTargetShopData());
        }

        public string getId()
        {
            return "";
        }
    }
}
