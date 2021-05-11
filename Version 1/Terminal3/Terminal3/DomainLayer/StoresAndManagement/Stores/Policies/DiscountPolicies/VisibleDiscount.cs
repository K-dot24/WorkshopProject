using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class VisibleDiscount : AbstractDiscountPolicy
    {

        public DateTime ExpirationDate { get; }
        public IDiscountTarget Target { get; }
        public Double Percentage { get; }

        public VisibleDiscount(DateTime expirationDate, IDiscountTarget target, Double percentage, String id="") : base(id)
        {
            ExpirationDate = expirationDate;
            Target = target;
            if (percentage > 100)
                Percentage = 100;
            else if (percentage < 0)
                Percentage = 0;
            else
                Percentage = percentage;
        }

        public override Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            //if the discount is expired
            if (DateTime.Now.CompareTo(ExpirationDate) >= 0)
                return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());

            List<Product> targetProducts = Target.getTargets(products);
            Dictionary<Product, Double> resultDictionary = new Dictionary<Product, Double>();
            foreach(Product product in targetProducts)
            {
                resultDictionary.Add(product, Percentage);
            }

            return new Result<Dictionary<Product, double>>("", true, resultDictionary);
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
                return new Result<bool>("Can't add a discount to a visible discount with an id " + id, false, false);
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveDiscount(String id)
        {
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

        public override Result<IDiscountPolicyData> GetData()
        {
            IDiscountTargetData targetData = null;
            if (Target != null)
            {
                Result<IDiscountTargetData> targetDataResult = Target.GetData();
                if (!targetDataResult.ExecStatus)
                    return new Result<IDiscountPolicyData>(targetDataResult.Message, false, null);
                targetData = targetDataResult.Data;
            }

            return new Result<IDiscountPolicyData>("", true, new VisibleDiscountData(ExpirationDate, targetData, Percentage, Id));
        }
    }
}
