using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountTargetCategories : IDiscountTarget
    {
        public List<string> Categories { get; }

        public DiscountTargetCategories(List<string> categories)
        {
            Categories = categories;
        }

        public static Result<IDiscountTarget> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create DiscountTargetCategories: ";
            if (!info.ContainsKey("Categories"))
                return new Result<IDiscountTarget>(errorMsg + "Categories not found", false, null);
            List<string> categories = createCategoriesList((JsonElement)info["Categories"]);

            return new Result<IDiscountTarget>("", true, new DiscountTargetCategories(categories));
        }

        private static List<string> createCategoriesList(JsonElement categoriesElement)
        {
            List<string> categories = JsonSerializer.Deserialize<List<string>>(categoriesElement.GetRawText());
            return categories;
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
    }
}
