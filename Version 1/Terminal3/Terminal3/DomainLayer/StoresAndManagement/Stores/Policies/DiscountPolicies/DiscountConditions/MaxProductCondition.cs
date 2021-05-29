using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MaxProductCondition : AbstractDiscountCondition
    {

        public int MaxQuantity { set; get; }
        public Product Product { set; get; }

        public MaxProductCondition(Product product, int maxQuantity, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Product = product;
            MaxQuantity = maxQuantity;
        }

        public static Result<IDiscountCondition> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MaxProductCondition: ";
            if (!info.ContainsKey("MaxQuantity"))
                return new Result<IDiscountCondition>(errorMsg + "MaxQuantity not found", false, null);
            int maxQuantity = (int)info["MaxQuantity"];

            if (!info.ContainsKey("Product"))
                return new Result<IDiscountCondition>("Product not found", false, null);
            Product product = (Product)info["Product"];

            return new Result<IDiscountCondition>("", true, new MaxProductCondition(product, maxQuantity));
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            if (products.ContainsKey(Product) && products[Product] <= MaxQuantity)
                return new Result<bool>("", true, true);
            return new Result<bool>("", true, false);
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
            return new Result<IDiscountConditionData>("", true, new MaxProductConditionData(Product.GetDAL().Data, MaxQuantity, Id));
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("MaxQuantity"))
                MaxQuantity = (int)info["MaxQuantity"];

            if (info.ContainsKey("Product"))
                Product = (Product)info["Product"];

            return new Result<bool>("", true, true);
        }
    }
}
