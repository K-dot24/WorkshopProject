using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountAnd : AbstractDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountAnd(String id = "") : base(id)
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public DiscountAnd(List<IDiscountPolicy> discounts, String id = "") : base(id)
        {
            Discounts = discounts;
            if (Discounts == null)
                Discounts = new List<IDiscountPolicy>();
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            List<Dictionary<Product, double>> results = new List<Dictionary<Product, double>>();

            foreach(IDiscountPolicy myDiscount in Discounts)
            {
                Result<Dictionary<Product, double>> result = myDiscount.CalculateDiscount(products, code);
                
                if (result.Data == null)
                    return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());
                if (result.Data.Count == 0)
                    return result;

                results.Add(result.Data);
            }

            return combineAndDiscounts(results);
        }

        private Result<Dictionary<Product, double>> combineAndDiscounts(List<Dictionary<Product, double>> discounts)
        {
            if (discounts == null)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());
            if (discounts.Count==0)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());

            Dictionary<Product, double> acc = discounts[0];

            foreach(Dictionary<Product, double> discount in discounts)
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
                return new Result<bool>("", true, true);
            }
            foreach (IDiscountPolicy myDiscount in Discounts)
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

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.AddCondition(id, condition);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.RemoveCondition(id);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountPolicyData> GetData()
        {
            List<IDiscountPolicyData> discountsList = new List<IDiscountPolicyData>();
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDiscountPolicyData> discountResult = myDiscount.GetData();
                if (!discountResult.ExecStatus)
                    return new Result<IDiscountPolicyData>(discountResult.Message, false, null);
                discountsList.Add(discountResult.Data);
            }
            return new Result<IDiscountPolicyData>("", true, new DiscountAndData(discountsList, Id));
        }
    }
}
