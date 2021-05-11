using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData
{
    class AbstractDiscountPolicyData : IDiscountPolicyData
    {
        public string Id { get; }

        public AbstractDiscountPolicyData(String id = "")
        {
            if (id.Equals(""))
                Id = Service.GenerateId();
            else
                Id = id;
        }

    }
}
