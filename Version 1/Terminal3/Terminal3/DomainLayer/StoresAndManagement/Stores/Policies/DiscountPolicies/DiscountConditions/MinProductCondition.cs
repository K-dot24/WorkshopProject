using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MinProductCondition : AbstractDiscountCondition
    {

        public int MinQuantity { set; get; }
        public Product Product { set; get; }

        public MinProductCondition(Product product, int minQuantity, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Product = product;
            MinQuantity = minQuantity;
        }

        public static Result<IDiscountCondition> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MinProductCondition: ";
            if (!info.ContainsKey("MinQuantity"))
                return new Result<IDiscountCondition>(errorMsg + "MinQuantity not found", false, null);
            int minQuantity = (int)info["MinQuantity"];

            if (!info.ContainsKey("Product"))
                return new Result<IDiscountCondition>("Product not found", false, null);
            Product product = (Product)info["Product"];

            return new Result<IDiscountCondition>("", true, new MinProductCondition(product, minQuantity));
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            if (products.ContainsKey(Product) && products[Product] >= MinQuantity)
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
            return new Result<IDiscountConditionData>("", true, new MinProductConditionData(Product.GetDAL().Data, MinQuantity, Id));
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("MinQuantity"))
                MinQuantity = (int)info["MinQuantity"];

            if (info.ContainsKey("Product"))
                Product = (Product)info["Product"];

            return new Result<bool>("", true, true);
        }
    }
}
