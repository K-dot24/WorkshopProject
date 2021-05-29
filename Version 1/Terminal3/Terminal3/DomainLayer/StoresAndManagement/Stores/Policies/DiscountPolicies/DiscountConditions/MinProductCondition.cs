using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MinProductCondition : AbstractDiscountCondition
    {

        public int MinQuantity { set; get; }
        public String ProductId { set; get; }

        public MinProductCondition(String productId, int minQuantity, String id = "") : base(new Dictionary<string, object>(), id)
        {
            ProductId = productId;
            MinQuantity = minQuantity;
        }

        public static Result<IDiscountCondition> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MinProductCondition: ";
            if (!info.ContainsKey("MinQuantity"))
                return new Result<IDiscountCondition>(errorMsg + "MinQuantity not found", false, null);
            int minQuantity = ((JsonElement)info["MinQuantity"]).GetInt32();

            if (!info.ContainsKey("ProductId"))
                return new Result<IDiscountCondition>("ProductId not found", false, null);
            String productId = ((JsonElement)info["ProductId"]).GetString();

            return new Result<IDiscountCondition>("", true, new MinProductCondition(productId, minQuantity));
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            Product myProduct = ContainsProduct(products);
            if(myProduct == null && MinQuantity == 0)
                return new Result<bool>("", true, true);
            if (products.ContainsKey(myProduct) && products[myProduct] >= MinQuantity)
                return new Result<bool>("", true, true);
            return new Result<bool>("", true, false);
        }

        private Product ContainsProduct(ConcurrentDictionary<Product, int> products)
        {
            foreach (KeyValuePair<Product, int> entry in products)
            {
                if (entry.Key.Id.Equals(ProductId))
                    return entry.Key;
            }
            return null;
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountConditionData> GetData()
        {
            return new Result<IDiscountConditionData>("", true, new MinProductConditionData(ProductId, MinQuantity, Id));
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("MinQuantity"))
                MinQuantity = ((JsonElement)info["MinQuantity"]).GetInt32();

            if (info.ContainsKey("ProductId"))
                ProductId = ((JsonElement)info["ProductId"]).GetString();

            return new Result<bool>("", true, true);
        }
    }
}
