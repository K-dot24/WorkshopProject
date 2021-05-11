using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class MinAgePolicyData : IPurchasePolicyData
    {
        public int Age { get; }
        public string Id { get; }

        public MinAgePolicyData(int age, string id)
        {
            this.Age = age;
            this.Id = id;
        }
    }
}
