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

        public Result<IDictionary<string, object>> GetData()
        {
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                {"type", "DiscountTargetProducts" },
                {"Categories", Categories }
            };
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public string getId()
        {
            return this.Id;
        }
    }
}
