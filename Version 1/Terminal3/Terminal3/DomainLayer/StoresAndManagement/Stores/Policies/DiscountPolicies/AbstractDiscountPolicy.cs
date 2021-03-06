using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public abstract class AbstractDiscountPolicy : IDiscountPolicy
    {
        public string Id { get; }

        public AbstractDiscountPolicy(Dictionary<string, object> info, String id = "")
        {
            if (id.Equals(""))
                Id = Service.GenerateId();
            else
                Id = id;
        }


        public abstract Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "");
        public abstract Result<bool> AddDiscount(String id, IDiscountPolicy discount);
        public abstract Result<IDiscountPolicy> RemoveDiscount(String id);
        public abstract Result<bool> AddCondition(string id, IDiscountCondition condition);
        public abstract Result<IDiscountCondition> RemoveCondition(string id);
        public abstract Result<bool> EditDiscount(Dictionary<string, object> info, string id);
        public abstract Result<bool> EditCondition(Dictionary<string, object> info, string id);
        public abstract Result<IDictionary<string, object>> GetData();
    }
}
