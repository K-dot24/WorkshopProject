using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountConditionOr : AbstractDiscountCondition
    {

        public List<IDiscountCondition> Conditions { get; }

        public DiscountConditionOr(String id = "") : base(new Dictionary<string, object>(), id)
        {
            Conditions = new List<IDiscountCondition>();
        }

        public static Result<IDiscountCondition> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountCondition>("", true, new DiscountConditionOr());
        }

        public DiscountConditionOr(List<IDiscountCondition> conditions, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Conditions = conditions;
            if (Conditions == null)
                Conditions = new List<IDiscountCondition>();
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            foreach (IDiscountCondition myCondition in Conditions)
            {
                Result<bool> result = myCondition.isConditionMet(products);
                if (result.ExecStatus && result.Data)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            if (Id.Equals(id))
            {
                Conditions.Add(condition);
                return new Result<bool>("", true, true);
            }
            foreach (IDiscountCondition myCondition in Conditions)
            {
                Result<bool> result = myCondition.AddCondition(id, condition);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountCondition> RemoveCondition(string id)
        {
            IDiscountCondition toBeRemoved = getCondition(id);
            if (toBeRemoved != null)
            {
                Conditions.Remove(toBeRemoved);
                return new Result<IDiscountCondition>("", true, toBeRemoved);
            }
            foreach (IDiscountCondition myCondition in Conditions)
            {
                Result<IDiscountCondition> result = myCondition.RemoveCondition(id);
                if (result.ExecStatus && result.Data != null)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<IDiscountCondition>("", true, null);
        }

        private IDiscountCondition getCondition(String id)
        {
            foreach (IDiscountCondition myCondition in Conditions)
            {
                if (myCondition.Id.Equals(id))
                    return myCondition;
            }
            return null;
        }

        public override Result<IDiscountConditionData> GetData()
        {
            List<IDiscountConditionData> conditionsList = new List<IDiscountConditionData>();
            foreach(IDiscountCondition myCondition in Conditions)
            {
                Result<IDiscountConditionData> conditionResult = myCondition.GetData();
                if (!conditionResult.ExecStatus)
                    return new Result<IDiscountConditionData>(conditionResult.Message, false, null);
                conditionsList.Add(conditionResult.Data);
            }
            return new Result<IDiscountConditionData>("", true, new DiscountConditionOrData(conditionsList, Id));
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Id != id)
            {
                foreach (IDiscountCondition myCondition in Conditions)
                {
                    Result<bool> result = myCondition.EditCondition(info, id);
                    if (result.ExecStatus && result.Data)
                        return result;
                    if (!result.ExecStatus)
                        return result;
                }
                return new Result<bool>("", true, false);
            }

            return new Result<bool>("", true, true);
        }
    }
}
