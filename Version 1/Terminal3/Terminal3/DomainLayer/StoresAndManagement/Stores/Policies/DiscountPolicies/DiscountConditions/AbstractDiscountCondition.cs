using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public abstract class AbstractDiscountCondition : IDiscountCondition
    {
        public string Id { get; }

        public AbstractDiscountCondition(String id = "")
        {
            if (id.Equals(""))
                Id = Service.GenerateId();
            else
                Id = id;
        }

        public abstract Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products);
        public abstract Result<bool> AddCondition(string id, IDiscountCondition condition);
        public abstract Result<bool> RemoveCondition(string id);
    }
}
