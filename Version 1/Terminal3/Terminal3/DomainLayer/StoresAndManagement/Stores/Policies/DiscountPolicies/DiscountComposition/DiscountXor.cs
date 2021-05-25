using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountXor : AbstractDiscountPolicy
    {

        public IDiscountPolicy Discount1 { get; }
        public IDiscountPolicy Discount2 { get; }
        public IDiscountCondition ChoosingCondition { get; }

        public DiscountXor(IDiscountPolicy discount1, IDiscountPolicy discount2, IDiscountCondition choosingCondition, String Id = "") : base(Id)
        {
            Discount1 = discount1;
            Discount2 = discount2;
            ChoosingCondition = choosingCondition;
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            Result<Dictionary<Product, double>> result1 = Discount1.CalculateDiscount(products, code);
            Result<Dictionary<Product, double>> result2 = Discount2.CalculateDiscount(products, code);

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

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            Result<bool> result = Discount1.AddCondition(id, condition);
            if (result.ExecStatus && result.Data)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.AddCondition(id, condition);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            Result<bool> result = Discount1.RemoveCondition(id);
            if (result.ExecStatus && result.Data)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.RemoveCondition(id);
        }

        public override Result<IDiscountPolicyData> GetData()
        {
            Result<IDiscountPolicyData> discount1Result = Discount1.GetData();
            if (!discount1Result.ExecStatus)
                return new Result<IDiscountPolicyData>(discount1Result.Message, false, null);
            Result<IDiscountPolicyData> discount2Result = Discount2.GetData();
            if (!discount2Result.ExecStatus)
                return new Result<IDiscountPolicyData>(discount2Result.Message, false, null);
            Result<IDiscountConditionData> choosingConditionResult = ChoosingCondition.GetData();
            if (!choosingConditionResult.ExecStatus)
                return new Result<IDiscountPolicyData>(choosingConditionResult.Message, false, null);
            return new Result<IDiscountPolicyData>("", true, new DiscountXorData(discount1Result.Data, discount2Result.Data, choosingConditionResult.Data, Id));
        }
    }
}
