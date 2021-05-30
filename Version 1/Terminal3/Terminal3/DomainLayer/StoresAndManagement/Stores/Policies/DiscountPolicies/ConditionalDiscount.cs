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

        public IDiscountCondition Condition { set; get; }
        public IDiscountPolicy Discount { set; get; }

        public ConditionalDiscount(IDiscountPolicy discount, IDiscountCondition condition, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Condition = condition;
            Discount = discount;
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountPolicy>("", true, new ConditionalDiscount(null, null));
        }

        public override Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            if(Condition == null || Discount == null)
                return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());
            
            Result<bool> isEligible = Condition.isConditionMet(products);
            if (isEligible.ExecStatus && isEligible.Data)
            {
                return Discount.CalculateDiscount(products, code);
            }
            return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id == id)
                Discount = discount;
            else if(Discount != null)
                return Discount.AddDiscount(id, discount);
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveDiscount(String id)
        {
            if (Discount.Id.Equals(id))
                Discount = null;
            else if (Discount != null)
                return Discount.RemoveDiscount(id);
            return new Result<bool>("", true, false);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            if (Id == id)
                Condition = condition;
            else if (Condition != null)
                return Condition.AddCondition(id, condition);
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            if (Condition == null)
                return new Result<bool>("", true, false);
            if (Condition.Id.Equals(id))
            {
                //return new Result<bool>("Cant remove the main condition of the conditional discount yet", false, false);
                Condition = null;
                return new Result<bool>("", true, true);
            }
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

        public override Result<bool> EditDiscount(Dictionary<string, object> info, string id)
        {
            if (Id != id)
            {
                if (Discount != null)
                    return Discount.EditDiscount(info, id);
                return new Result<bool>("", true, false);
            }

            return new Result<bool>("", true, true);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Discount != null)
                return Discount.EditCondition(info, id);
            return new Result<bool>("", true, false);
        }
    }
}
