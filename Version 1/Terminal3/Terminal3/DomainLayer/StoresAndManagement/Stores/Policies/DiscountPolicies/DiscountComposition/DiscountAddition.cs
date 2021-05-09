using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountAddition : AbstractDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountAddition(String id = "") : base(id)
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public DiscountAddition(List<IDiscountPolicy> discounts, String id = "") : base(id)
        {
            Discounts = discounts;
            if (Discounts == null)
                Discounts = new List<IDiscountPolicy>();
        }

        public override Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Dictionary<Product, Double> result = new Dictionary<Product, Double>();

            foreach(IDiscountPolicy discountPolicy in Discounts)
            {
                Dictionary<Product, Double> discountResult = discountPolicy.CalculateDiscount(products).Data;
                if (discountResult == null)
                    continue;
                foreach(KeyValuePair<Product, Double> entry in discountResult)
                {
                    if (result.ContainsKey(entry.Key))
                        result[entry.Key] = result[entry.Key] + entry.Value;
                    else
                        result[entry.Key] = entry.Value;
                }
            }

            return new Result<Dictionary<Product, Double>>("", true, result);
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
            {
                Discounts.Add(discount);
                return new Result<bool>("", true, true);
            }
            foreach(IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.AddDiscount(id, discount);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveDiscount(String id)
        {
            if (Discounts.RemoveAll(discount => discount.Id.Equals(id)) >= 1)
                return new Result<bool>("", true, true);
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.RemoveDiscount(id);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }
    }
}
