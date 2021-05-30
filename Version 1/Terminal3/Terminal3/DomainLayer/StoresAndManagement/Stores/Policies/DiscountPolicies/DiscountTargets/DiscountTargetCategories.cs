using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountTargetCategories : IDiscountTarget
    {
        public List<string> Categories { get; }
        public string Id { get; set; }
        public DiscountTargetCategories(List<string> categories)
        {
            Id = Service.GenerateId();
            Categories = categories;
        }

        // for loading from db
        public DiscountTargetCategories(List<string> categories, string id) 
        {
            Id = id;
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

        public Result<IDiscountTargetData> GetData()
        {
            return new Result<IDiscountTargetData>("", true, new DiscountTargetCategoriesData(new List<string>(Categories)));
        }

        public string getId()
        {
            return this.Id;
        }
    }
}
