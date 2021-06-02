using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition;


namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountAddition : AbstractDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountAddition(String id = "") : base(new Dictionary<string, object>(), id)
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountPolicy>("", true, new DiscountAddition());
        }

        public DiscountAddition(List<IDiscountPolicy> discounts, String id = "") : base(new Dictionary<string, object>(), id)
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
                Dictionary<Product, Double> discountResult = discountPolicy.CalculateDiscount(products, code).Data;
                if (discountResult == null)
                    continue;
                foreach(KeyValuePair<Product, Double> entry in discountResult)
                {
                    if (result.ContainsKey(entry.Key))
                        if (result[entry.Key] + entry.Value > 100)
                            result[entry.Key] = 100;
                        else
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

        public override Result<IDiscountPolicy> RemoveDiscount(String id)
        {
            IDiscountPolicy toBeRemoved = getDiscount(id);
            if(toBeRemoved != null)
            {
                Discounts.Remove(toBeRemoved);
                return new Result<IDiscountPolicy>("", true, toBeRemoved);
            }
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDiscountPolicy> result = myDiscount.RemoveDiscount(id);
                if (result.ExecStatus && result.Data != null)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<IDiscountPolicy>("", true, null);
        }

        private IDiscountPolicy getDiscount(String id)
        {
            foreach(IDiscountPolicy myDiscount in Discounts)
            {
                if (myDiscount.Id.Equals(id))
                    return myDiscount;
            }
            return null;
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

        public override Result<IDiscountCondition> RemoveCondition(string id)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDiscountCondition> result = myDiscount.RemoveCondition(id);
                if (result.ExecStatus && result.Data != null)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<IDiscountCondition>("", true, null);
        }

        public override Result<IDictionary<string, object>> GetData()
        {
            IDictionary<string, object> dict = new Dictionary<string, object>() { 
                {"type", "DiscountAddition" },
                {"Id", Id },
                {"Discounts", null }
            };
            List<IDictionary<string, object>> discountsList = new List<IDictionary<string, object>>();
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDictionary<string, object>> discountResult = myDiscount.GetData();
                if (!discountResult.ExecStatus)
                    return discountResult;
                discountsList.Add(discountResult.Data);
            }
            dict["Discounts"] = discountsList;
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public override Result<bool> EditDiscount(Dictionary<string, object> info, string id)
        {
            if (Id != id)
            {
                foreach (IDiscountPolicy myDiscount in Discounts)
                {
                    Result<bool> result = myDiscount.EditCondition(info, id);
                    if (result.ExecStatus && result.Data)
                        return result;
                    if (!result.ExecStatus)
                        return result;
                }
                return new Result<bool>("", true, false);
            }

            return new Result<bool>("", true, true);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.EditCondition(info, id);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public DataAccessLayer.DTOs.DTO_DiscountAddition getDTO()
        {
            return null; // TODO - DTO_DiscountAddition
        }
    }
}
