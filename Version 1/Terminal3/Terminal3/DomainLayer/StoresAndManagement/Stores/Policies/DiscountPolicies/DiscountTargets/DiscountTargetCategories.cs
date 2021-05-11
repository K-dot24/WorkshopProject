using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountTargetCategories : IDiscountTarget
    {
        public List<string> Categories { get; }

        public DiscountTargetCategories(List<string> categories)
        {
            Categories = categories;
        }

        public List<Product> getTargets(ConcurrentDictionary<Product, int> products)
        {
            List<Product> result = new List<Product>();
            foreach (KeyValuePair<Product, int> entry in products)
            {
                if (Categories.Contains(entry.Key.Category))
                    result.Add(entry.Key);
            }
            return result;
        }
    }
}
