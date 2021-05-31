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

        public IDiscountPolicy Discount1 { set; get; }
        public IDiscountPolicy Discount2 { set; get; }
        public IDiscountCondition ChoosingCondition { set; get; }

        public DiscountXor(IDiscountPolicy discount1, IDiscountPolicy discount2, IDiscountCondition choosingCondition, String Id = "") : base(new Dictionary<string, object>(), Id)
        {
            Discount1 = discount1;
            Discount2 = discount2;
            ChoosingCondition = choosingCondition;
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountPolicy>("", true, new DiscountXor(null, null, null));
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            if (Discount1 == null || Discount2 == null || ChoosingCondition == null)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());

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
            {
                if (Discount1 == null)
                    Discount1 = discount;
                else if (Discount2 == null)
                    Discount2 = discount;
                else
                    return new Result<bool>("Can't add a discount to a full xor", false, false);
                return new Result<bool>("", true, true);
            }

            Result<bool> result = Discount1.AddDiscount(id, discount);
            if (result.ExecStatus && result.Data)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.AddDiscount(id, discount);
        }

        public override Result<IDiscountPolicy> RemoveDiscount(string id)
        {
            IDiscountPolicy oldPolicy = null;
            if (Discount1 != null && Discount1.Id.Equals(id))
            {
                oldPolicy = Discount1;
                Discount1 = null;
                return new Result<IDiscountPolicy>("", true, oldPolicy);
            }
            if (Discount2 != null && Discount2.Id.Equals(id))
            {
                oldPolicy = Discount2;
                Discount2 = null;
                return new Result<IDiscountPolicy>("", true, oldPolicy);
            }

            Result<IDiscountPolicy> result = Discount1.RemoveDiscount(id);
            if (result.ExecStatus && result.Data != null)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.RemoveDiscount(id);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            if(Id == id)
            {
                ChoosingCondition = condition;
                return new Result<bool>("", true, true);
            }
            if(ChoosingCondition != null)
            {
                Result<bool> choosingResult = ChoosingCondition.AddCondition(id, condition);
                if (choosingResult.ExecStatus && choosingResult.Data)
                    return choosingResult;
                if (!choosingResult.ExecStatus)
                    return choosingResult;
            }
            Result<bool> result = Discount1.AddCondition(id, condition);
            if (result.ExecStatus && result.Data)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.AddCondition(id, condition);
        }

        public override Result<IDiscountCondition> RemoveCondition(string id)
        {
            if (ChoosingCondition != null && ChoosingCondition.Id == id)
            {
                IDiscountCondition oldCondition = ChoosingCondition;
                ChoosingCondition = null;
                return new Result<IDiscountCondition>("", true, oldCondition);
            }
            if (ChoosingCondition != null)
            {
                Result<IDiscountCondition> choosingResult = ChoosingCondition.RemoveCondition(id);
                if (choosingResult.ExecStatus && choosingResult.Data != null)
                    return choosingResult;
                if (!choosingResult.ExecStatus)
                    return choosingResult;
            }
            Result<IDiscountCondition> result = Discount1.RemoveCondition(id);
            if (result.ExecStatus && result.Data != null)
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

        public override Result<bool> EditDiscount(Dictionary<string, object> info, string id)
        {
            if (Id != id)
            {
                Result<bool> result = Discount1.EditDiscount(info, id);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
                return Discount2.EditDiscount(info, id);
            }

            return new Result<bool>("", true, true);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (ChoosingCondition != null)
            {
                Result<bool> choosingResult = ChoosingCondition.EditCondition(info, id);
                if (choosingResult.ExecStatus && choosingResult.Data)
                    return choosingResult;
                if (!choosingResult.ExecStatus)
                    return choosingResult;
            }
            Result<bool> result = Discount1.EditCondition(info, id);
            if (result.ExecStatus && result.Data)
                return result;
            if (!result.ExecStatus)
                return result;
            return Discount2.EditCondition(info, id);
        }
    }
}
