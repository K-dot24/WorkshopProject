using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountConditionOr : IDiscountCondition
    {

        public IDiscountCondition Condition1 { get; }
        public IDiscountCondition Condition2 { get; }

        public DiscountConditionOr(IDiscountCondition condition1, IDiscountCondition condition2)
        {
            Condition1 = condition1;
            Condition2 = condition2;
        }

        public Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            Result<bool> isEligible1 = Condition1.isConditionMet(products);
            Result<bool> isEligible2 = Condition2.isConditionMet(products);
            if ((isEligible1.ExecStatus && isEligible1.Data) || (isEligible2.ExecStatus && isEligible2.Data))
                return new Result<bool>("", true, true);
            return new Result<bool>("", true, false);
        }

    }
}
