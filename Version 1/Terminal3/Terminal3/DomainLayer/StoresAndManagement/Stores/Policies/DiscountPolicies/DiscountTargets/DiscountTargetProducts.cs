using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountTargets
{
    public class DiscountTargetProducts : IDiscountTarget
    {
        public String Id { get; set; }
        public List<string> ProductIds { get; }

        public DiscountTargetProducts(List<string> productIds)
        {
            Id = Service.GenerateId();
            ProductIds = productIds;
        }

        // for loading from db
        public DiscountTargetProducts(string id, List<string> productIds)
        {
            Id = id;
            ProductIds = productIds;
        }

        public static Result<IDiscountTarget> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create DiscountTargetProducts: ";
            if (!info.ContainsKey("ProductIds"))
                return new Result<IDiscountTarget>(errorMsg + "ProductIds not found", false, null);
            List<string> productIds = createProductsList((JsonElement)info["ProductIds"]);

            return new Result<IDiscountTarget>("", true, new DiscountTargetProducts(productIds));
        }

        private static List<string> createProductsList(JsonElement productsElement)
        {
            List<string> products = JsonSerializer.Deserialize<List<string>>(productsElement.GetRawText());
            return products;
        }

        public List<Product> getTargets(ConcurrentDictionary<Product, int> products)
        {
            List<Product> result = new List<Product>();
            foreach(KeyValuePair<Product, int> entry in products)
            {
                if (ProductIds.Contains(entry.Key.Id))
                    result.Add(entry.Key);
            }
            return result;
        }

        public Result<IDiscountTargetData> GetData()
        {
            List<string> productList = new List<string>();
            foreach(string myProductId in ProductIds)
                productList.Add(myProductId);
            return new Result<IDiscountTargetData>("", true, new DiscountTargetProductsData(productList));
        }

        public string getId()
        {
            return this.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is DiscountTargetProducts products &&
                   Id == products.Id &&
                   EqualityComparer<List<string>>.Default.Equals(ProductIds, products.ProductIds);
        }
    }
}
