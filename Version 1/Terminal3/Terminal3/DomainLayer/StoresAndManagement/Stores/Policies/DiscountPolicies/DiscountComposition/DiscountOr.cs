using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountOr : AbstractDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountOr(String id = "") : base(id)
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public DiscountOr(List<IDiscountPolicy> discounts, String id = "") : base(id)
        {
            Discounts = discounts;
            if (Discounts == null)
                Discounts = new List<IDiscountPolicy>();
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            List<Dictionary<Product, double>> results = new List<Dictionary<Product, double>>();

            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<Dictionary<Product, double>> result = myDiscount.CalculateDiscount(products, code);

                if (result.Data == null)
                    result.Data=new Dictionary<Product, double>();

                results.Add(result.Data);
            }

            return combineOrDiscounts(results);
        }

        private Result<Dictionary<Product, double>> combineOrDiscounts(List<Dictionary<Product, double>> discounts)
        {
            if (discounts == null)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());
            if (discounts.Count == 0)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());

            Dictionary<Product, double> acc = discounts[0];

            foreach (Dictionary<Product, double> discount in discounts)
            {
                foreach (KeyValuePair<Product, double> entry in discount)
                {
                    if (!acc.ContainsKey(entry.Key))
                        acc[entry.Key] = entry.Value;
                    else if (acc[entry.Key] < entry.Value)
                        acc[entry.Key] = entry.Value;
                }
            }

            return new Result<Dictionary<Product, double>>("", true, acc);
        }
        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
            {
                Discounts.Add(discount);
                return new Result<bool>("Successfully added the policy to the id of " + id, true, true);
            }
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.AddDiscount(id, discount);
                if (result.ExecStatus && result.Data)
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
            }
            return new Result<bool>("", true, false);
        }
    }
}
