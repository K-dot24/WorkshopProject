using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountTargets
{
    public class DiscountTargetProducts : IDiscountTarget
    {

        public List<Product> Products { get; }

        public DiscountTargetProducts(List<Product> products)
        {
            Products = products;
        }

        public List<Product> getTargets(ConcurrentDictionary<Product, int> products)
        {
            List<Product> result = new List<Product>();
            foreach(Product product in Products)
            {
                if (products.ContainsKey(product))
                    result.Add(product);
            }
            return result;
        }
    }
}
