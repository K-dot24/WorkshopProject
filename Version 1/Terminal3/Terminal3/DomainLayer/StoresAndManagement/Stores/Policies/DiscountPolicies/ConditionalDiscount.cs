using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class ConditionalDiscount : AbstractDiscountPolicy
    {

        public IDiscountCondition Condition { get; }
        public IDiscountPolicy Discount { get; }

        public ConditionalDiscount(IDiscountPolicy discount, IDiscountCondition condition, String id = "") : base(id)
        {
            Condition = condition;
            Discount = discount;
        }

        public override Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<bool> isEligible = Condition.isConditionMet(products);
            if (isEligible.ExecStatus && isEligible.Data)
            {
                return Discount.CalculateDiscount(products, code);
            }
            return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            return Discount.AddDiscount(id, discount);
        }

        public override Result<bool> RemoveDiscount(String id)
        {
            return Discount.RemoveDiscount(id);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            return Condition.AddCondition(id, condition);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            if (Condition.Id.Equals(id))
                return new Result<bool>("Cant remove the main condition of the conditional discount yet", false, false);
            return Condition.RemoveCondition(id);
        }

        public override Result<IDiscountPolicyData> GetData()
        {
            IDiscountPolicyData discountData = null;
            if (Discount != null)
            {
                Result<IDiscountPolicyData> discountDataResult = Discount.GetData();
                if (!discountDataResult.ExecStatus)
                    return discountDataResult;
                discountData = discountDataResult.Data;
            }
            IDiscountConditionData conditionData = null;
            if (Condition != null)
            {
                Result<IDiscountConditionData> conditionDataResult = Condition.GetData();
                if (!conditionDataResult.ExecStatus)
                    return new Result<IDiscountPolicyData>(conditionDataResult.Message, false, null);
                conditionData = conditionDataResult.Data;
            }

            return new Result<IDiscountPolicyData>("", true, new ConditionalDiscountData(discountData, conditionData, Id));
        }
    }
}
