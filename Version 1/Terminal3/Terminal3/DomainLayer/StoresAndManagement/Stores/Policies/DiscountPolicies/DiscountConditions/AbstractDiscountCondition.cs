using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public abstract class AbstractDiscountCondition : IDiscountCondition
    {
        public string Id { get; }

        public AbstractDiscountCondition(Dictionary<string, object> info, String id = "")
        {
            if (id.Equals(""))
                Id = Service.GenerateId();
            else
                Id = id;
        }

        public abstract Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products);
        public abstract Result<bool> AddCondition(string id, IDiscountCondition condition);
        public abstract Result<bool> RemoveCondition(string id);
        public abstract Result<bool> EditCondition(Dictionary<string, object> info, string id);
        public abstract Result<IDiscountConditionData> GetData();
    }
}
