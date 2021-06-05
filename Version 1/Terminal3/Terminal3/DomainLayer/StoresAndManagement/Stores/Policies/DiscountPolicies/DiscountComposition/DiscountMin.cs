using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountComposition
{
    public class DiscountMin : AbstractDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountMin(String id = "") : base(new Dictionary<string, object>(), id)
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountPolicy>("", true, new DiscountMin());
        }

        public DiscountMin(List<IDiscountPolicy> discounts, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Discounts = discounts;
            if (Discounts == null)
                Discounts = new List<IDiscountPolicy>();
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            //if we dont have discounts
            if (Discounts.Count == 0)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());

            //calculating all discounts
            List<Dictionary<Product, Double>> discountsResultsList = CalculateAllDiscounts(products, code);

            //choosing the biggest discount
            return new Result<Dictionary<Product, double>>("", true, ChooseDiscountByResult(discountsResultsList, products));
        }

        private List<Dictionary<Product, Double>> CalculateAllDiscounts(ConcurrentDictionary<Product, int> products, String code)
        {
            List<Dictionary<Product, Double>> discountsResultsList = new List<Dictionary<Product, double>>();
            foreach (IDiscountPolicy discount in Discounts)
            {
                Dictionary<Product, Double> discountResultDictionary = discount.CalculateDiscount(products, code).Data;
                if (discountResultDictionary == null)
                    discountResultDictionary = new Dictionary<Product, double>();
                if (discountResultDictionary.Count != 0)
                    discountsResultsList.Add(discountResultDictionary);
            }
            return discountsResultsList;
        }

        private Dictionary<Product, double> ChooseDiscountByResult(List<Dictionary<Product, Double>> discountsResultsList, ConcurrentDictionary<Product, int> products)
        {
            if (discountsResultsList.Count == 0)
                return new Dictionary<Product, double>();
            Dictionary<Product, double> chosenDiscount = discountsResultsList[0];
            Double chosenValue = CalculateDiscountsValue(chosenDiscount, products);

            foreach (Dictionary<Product, double> discount in discountsResultsList)
            {
                Double currDiscountValue = CalculateDiscountsValue(discount, products);
                if (currDiscountValue < chosenValue)
                {
                    chosenValue = currDiscountValue;
                    chosenDiscount = discount;
                }
            }
            return chosenDiscount;
        }

        private Double CalculateDiscountsValue(Dictionary<Product, Double> discountResult, ConcurrentDictionary<Product, int> products)
        {
            Double acc = 0;
            foreach (KeyValuePair<Product, Double> entry in discountResult)
            {
                acc += entry.Value * entry.Key.Price * products[entry.Key];
            }
            return acc;
        }

        public override Result<bool> AddDiscount(string id, IDiscountPolicy discount)
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

        public override Result<IDiscountPolicy> RemoveDiscount(String id)
        {
            IDiscountPolicy toBeRemoved = getDiscount(id);
            if (toBeRemoved != null)
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
            foreach (IDiscountPolicy myDiscount in Discounts)
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
            /*IDictionary<string, object> dict = new Dictionary<string, object>() { 
                {"type", "DiscountAddition" },
                {"Id", Id },
                {"Discounts", null }
            };*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", "Min"},
                { "children", new Dictionary<String, object>[0] }
            };
            List<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDictionary<string, object>> discountResult = myDiscount.GetData();
                if (!discountResult.ExecStatus)
                    return discountResult;
                children.Add(discountResult.Data);
            }
            dict["children"] = children.ToArray();
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
    }
}
