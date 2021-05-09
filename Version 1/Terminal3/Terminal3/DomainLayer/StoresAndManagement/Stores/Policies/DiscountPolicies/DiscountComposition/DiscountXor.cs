using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountXor : AbstractDiscountPolicy
    {

        public IDiscountPolicy Discount1 { get; }
        public IDiscountPolicy Discount2 { get; }
        public IDiscountCondition ChoosingCondition { get; }

        public DiscountXor(IDiscountPolicy discount1, IDiscountPolicy discount2, IDiscountCondition choosingCondition)
        {
            Discount1 = discount1;
            Discount2 = discount2;
            ChoosingCondition = choosingCondition;
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<Dictionary<Product, double>> result1 = Discount1.CalculateDiscount(products);
            Result<Dictionary<Product, double>> result2 = Discount2.CalculateDiscount(products);

            if (result1.Data == null)
                return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());
            if (result2.Data == null)
                return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());

            if (result1.Data.Count == 0)
                return result2;
            if (result2.Data.Count == 0)
                return result1;

            Result<bool> conditionResult = ChoosingCondition.isConditionMet(products);
            if (conditionResult.ExecStatus && conditionResult.Data)
                return result1;
            return result2;
        }

        public override Result<bool> AddDiscount(string id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
                return new Result<bool>("Can't add a discount to a xor with an id " + id, false, false);

            Result<bool> result = Discount1.AddDiscount(id, discount);
            if (result.ExecStatus && result.Data)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.AddDiscount(id, discount);
        }

        public override Result<bool> RemoveDiscount(string id)
        {
            if (Discount1.Id.Equals(id))
                return new Result<bool>("Can't remove a discount that is a child of xor. Id of the discount is " + id, false, false);
            if (Discount2.Id.Equals(id))
                return new Result<bool>("Can't remove a discount that is a child of xor. Id of the discount is " + id, false, false);

            Result<bool> result = Discount1.RemoveDiscount(id);
            if (result.ExecStatus && result.Data)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.RemoveDiscount(id);
        }
    }
}
