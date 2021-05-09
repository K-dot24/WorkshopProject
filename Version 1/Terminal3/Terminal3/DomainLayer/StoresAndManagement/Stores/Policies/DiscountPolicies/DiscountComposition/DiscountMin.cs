using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountComposition
{
    public class DiscountMin : AbstractDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountMin(String id = "") : base(id)
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public DiscountMin(List<IDiscountPolicy> discounts, String id = "") : base(id)
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
            List<Dictionary<Product, Double>> discountsResultsList = CalculateAllDiscounts(products);

            //choosing the biggest discount
            return new Result<Dictionary<Product, double>>("", true, ChooseDiscountByResult(discountsResultsList, products));
        }

        private List<Dictionary<Product, Double>> CalculateAllDiscounts(ConcurrentDictionary<Product, int> products)
        {
            List<Dictionary<Product, Double>> discountsResultsList = new List<Dictionary<Product, double>>();
            foreach (IDiscountPolicy discount in Discounts)
            {
                Dictionary<Product, Double> discountResultDictionary = discount.CalculateDiscount(products).Data;
                if (discountResultDictionary == null)
                    discountResultDictionary = new Dictionary<Product, double>();
                discountsResultsList.Add(discountResultDictionary);
            }
            return discountsResultsList;
        }

        private Dictionary<Product, double> ChooseDiscountByResult(List<Dictionary<Product, Double>> discountsResultsList, ConcurrentDictionary<Product, int> products)
        {
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
                return new Result<bool>("Successfully added the policy to the id of " + id, true, true);
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

        public override Result<bool> RemoveDiscount(string id)
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
